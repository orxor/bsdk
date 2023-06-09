unit Json;

interface

uses Windows,Classes,Variants,ActiveX,SysUtils,DateUtils;

type
  TextWriter=class
  public
    procedure Write(Value:String);virtual;abstract;
    procedure WriteLine;virtual;abstract;
  public
    constructor Create;
  end;

  ConsoleTextWriter=class(TextWriter)
  public
    procedure Write(Value:String);override;
    procedure WriteLine;override;
  public
    constructor Create;
  end;

  JsonContainerType=(
    JsonContainerTypeNone,JsonContainerTypeObject,
    JsonContainerTypeArray,JsonContainerTypeConstructor);

  JsonPosition=class
  public
    Position:Integer;
    Container:JsonContainerType;
    HasIndex:Boolean;
    PropertyName:String;
  public
    constructor Create;overload;
    constructor Create(Container:JsonContainerType);overload;
  private
    class function TypeHasIndex(Container:JsonContainerType):Boolean;
  end;

  JsonState=(JsonStateStart=0,JsonStateProperty,JsonStateObjectStart,
      JsonStateObject,JsonStateArrayStart,JsonStateArray,
      JsonStateConstructorStart,JsonStateConstructor,
      JsonStateClosed,JsonStateError);

  JsonToken=(JsonTokenNone=0,JsonTokenStartObject,JsonTokenStartArray,
      JsonTokenStartConstructor,JsonTokenPropertyName,JsonTokenComment,
      JsonTokenRaw,JsonTokenInteger,JsonTokenFloat,JsonTokenString,
      JsonTokenBoolean,JsonTokenNull,JsonTokenUndefined,JsonTokenEndObject,
      JsonTokenEndArray,JsonTokenEndConstructor,JsonTokenDate,JsonTokenBytes);

  JsonWriter=class
  private
    Formatting:Integer;
    CurrentPosition:JsonPosition;
    Stack:TList;
    CurrentState:JsonState;
  public
    constructor Create;
    destructor Destroy;override;
  public
    procedure WriteEnd;overload;
    procedure WritePropertyName(PropertyName:String);virtual;
    procedure WriteStartArray;virtual;
    procedure WriteEndArray;virtual;
    procedure WriteStartObject;virtual;
    procedure WriteStartConstructor(ClassName:String);virtual;
    procedure WriteEndObject;virtual;
    procedure WriteEndConstructor;virtual;
    procedure WriteValue(Value:OleVariant);overload;virtual;
    procedure WriteValue(Value:Integer);overload;virtual;
    procedure WriteValue(Value:Boolean);overload;virtual;
    procedure WriteValue(Value:String);overload;virtual;
    procedure WriteValue(Value:PWideChar);overload;virtual;
    procedure WriteProperty(PropertyName:String;Value:PWideChar);overload;
    procedure WriteProperty(PropertyName:String;Value:OleVariant);overload;
    procedure WriteProperty(PropertyName:String;Value:FILETIME);overload;
    procedure WriteProperty(PropertyName:String;Value:PBStrList;const Count:Integer);overload;
    procedure WriteNull;virtual;
    procedure WriteRaw(Value:String);virtual;abstract;
  protected
    procedure WriteValueDelimiter;virtual;abstract;
    procedure WriteIndentSpace;virtual;abstract;
    procedure WriteIndent;virtual;abstract;
  protected
    procedure WriteEndInternal(Container:JsonContainerType);overload;virtual;
    procedure WriteEndInternal(Token:JsonToken);overload;virtual;abstract;
    procedure InternalWriteStart(Token:JsonToken;Container:JsonContainerType);
    procedure InternalWriteValue(Token:JsonToken);overload;
    procedure InternalWriteValue(Token:JsonToken;Value:String;Quote:Boolean);overload;virtual;
    procedure InternalWriteEnd(Container:JsonContainerType);
    procedure AutoCompleteClose(Container:JsonContainerType);virtual;
    procedure UpdateScopeWithFinishedValue;
    procedure AutoComplete(Token:JsonToken);
    procedure Push(Container:JsonContainerType);
    function Pop:JsonContainerType;
    function Peek:JsonContainerType;
    function Top:Integer;
    function CalculateLevelsToComplete(Container:JsonContainerType):Integer;
    procedure UpdateCurrentState;
  protected
    class function GetCloseTokenForType(Container:JsonContainerType):JsonToken;
  end;

  JsonTextWriter=class(JsonWriter)
  private
    Writer:TextWriter;
    Indentation:Integer;
  public
    constructor Create(Writer:TextWriter);
    procedure WriteStartObject;override;
    procedure WriteStartArray;override;
    procedure WriteStartConstructor(ClassName:String);override;
    procedure WritePropertyName(PropertyName:String);override;
    procedure WriteValue(Value:Integer);overload;override;
    procedure WriteValue(Value:Boolean);overload;override;
    procedure WriteValue(Value:String);overload;override;
    procedure WriteRaw(Value:String);override;
  private
    procedure WriteEscapedString(Value:String;Quote:Boolean);
    procedure InternalWriteValue(Token:JsonToken;Value:String;Quote:Boolean);overload;override;
  protected
    procedure WriteEndInternal(Token:JsonToken);override;
    procedure WriteIndent;override;
    procedure WriteIndentSpace;override;
    procedure WriteValueDelimiter;override;
  end;

implementation

uses Contnrs;

const
  FormattingNone = 0;
  FormattingIndented = 1;

type
  TJsonStateArray=array[0..9] of JsonState;
var
  StateArrayTemplate:array[0..7] of TJsonStateArray;
  StateArray:array of TJsonStateArray;

{ JsonWriter }

procedure JsonWriter.AutoComplete(Token:JsonToken);
var
  State:JsonState;
begin
  State := StateArray[Integer(Token)][Integer(CurrentState)];
  if State <> JsonStateError then
  begin
    if ((CurrentState = JsonStateObject) or
        (CurrentState = JsonStateArray) or
        (CurrentState = JsonStateConstructor)) and
        (Token <> JsonTokenComment) then
    begin
      WriteValueDelimiter;
    end;
    if Formatting = FormattingIndented then
    begin
      if CurrentState = JsonStateProperty then WriteIndentSpace;
      if (CurrentState = JsonStateArray) or
         (CurrentState = JsonStateArrayStart) or
         ((CurrentState = JsonStateConstructor) or (CurrentState = JsonStateConstructorStart)) or
         (Token = JsonTokenPropertyName) and (CurrentState <> JsonStateStart) then
      begin
        WriteIndent;
      end;
    end;
    CurrentState := State;
  end;
end;

procedure JsonWriter.AutoCompleteClose(Container:JsonContainerType);
var
  I,LevelsToComplete:Integer;
  CloseTokenForType:JsonToken;
begin
  LevelsToComplete := CalculateLevelsToComplete(Container);
  for I := 0 to LevelsToComplete-1 do
  begin
    CloseTokenForType := GetCloseTokenForType(Pop);
    if CurrentState = JsonStateProperty then WriteNull;
    if (Formatting = FormattingIndented) or (CurrentState <> JsonStateObjectStart) and (CurrentState <> JsonStateArrayStart) then WriteIndent;
    WriteEndInternal(CloseTokenForType);
    UpdateCurrentState;
  end;
end;

constructor JsonWriter.Create;
begin
  CurrentPosition := JsonPosition.Create;
  Stack := TObjectList.Create(False);
  CurrentState := JsonStateStart;
  Formatting := FormattingIndented;
end;

procedure JsonWriter.Push(Container:JsonContainerType);
begin
  if CurrentPosition.Container <> JsonContainerTypeNone then
  begin
    Stack.Add(CurrentPosition);
  end
  else
  begin
    FreeAndNil(CurrentPosition);
  end;
  CurrentPosition := JsonPosition.Create(Container);
end;

function JsonWriter.Pop:JsonContainerType;
begin
  Result := CurrentPosition.Container;
  FreeAndNil(CurrentPosition);
  if Stack.Count > 0 then
  begin
    CurrentPosition := Stack[Stack.Count-1];
    Stack.Delete(Stack.Count-1);
  end
  else
  begin
    CurrentPosition := JsonPosition.Create;
  end;
end;

procedure JsonWriter.UpdateScopeWithFinishedValue;
begin
  if CurrentPosition.HasIndex then
  begin
    Inc(CurrentPosition.Position);
  end;
end;

procedure JsonWriter.WriteEndArray;
begin
  AutoCompleteClose(JsonContainerTypeArray);
end;

procedure JsonWriter.WriteEndConstructor;
begin
  AutoCompleteClose(JsonContainerTypeConstructor);
end;

procedure JsonWriter.WriteEndObject;
begin
  AutoCompleteClose(JsonContainerTypeObject);
end;

destructor JsonWriter.Destroy;
begin
  FreeAndNil(Stack);
  inherited;
end;

function JsonWriter.CalculateLevelsToComplete(Container:JsonContainerType):Integer;
var
  I,Offset:Integer;
begin
  Result := 0;
  if CurrentPosition.Container = Container then Result := 1
  else
  begin
    Offset := Top - 2;
    for I := Offset downto 0 do
    begin
      if JsonPosition(Stack[Offset-I]).Container = Container then
      begin
        Result := I + 2;
        Break;
      end;
    end;
  end;
end;

function JsonWriter.Top:Integer;
begin
  Result := Stack.Count;
  if Peek <> JsonContainerTypeNone then Inc(Result);
end;

function JsonWriter.Peek:JsonContainerType;
begin
  Result := CurrentPosition.Container;
end;

class function JsonWriter.GetCloseTokenForType(Container:JsonContainerType):JsonToken;
begin
  Result := JsonTokenNone;
  case Container of
    JsonContainerTypeObject: Result := JsonTokenEndObject;
    JsonContainerTypeArray:  Result := JsonTokenEndArray;
    JsonContainerTypeConstructor: Result := JsonTokenEndConstructor;
  end;
end;

procedure JsonWriter.UpdateCurrentState;
begin
  case Peek of
    JsonContainerTypeNone:   CurrentState := JsonStateStart;
    JsonContainerTypeObject: CurrentState := JsonStateObject;
    JsonContainerTypeArray:  CurrentState := JsonStateArray;
    JsonContainerTypeConstructor: CurrentState := JsonStateArray;
  end;
end;

procedure JsonWriter.InternalWriteValue(Token:JsonToken);
begin
  UpdateScopeWithFinishedValue;
  AutoComplete(Token);
end;

procedure JsonWriter.WriteNull;
begin
  InternalWriteValue(JsonTokenNull,'null',False);
end;

procedure JsonWriter.WriteEnd;
begin
  InternalWriteEnd(Peek);
end;

procedure JsonWriter.InternalWriteEnd(Container:JsonContainerType);
begin
  AutoCompleteClose(Container);
end;

procedure JsonWriter.WriteProperty(PropertyName:String;Value:PWideChar);
begin
  WritePropertyName(PropertyName);
  WriteValue(Value);
end;

procedure JsonWriter.WriteProperty(PropertyName:String;Value:OleVariant);
begin
  WritePropertyName(PropertyName);
  WriteValue(Value);
end;

procedure JsonWriter.WriteProperty(PropertyName:String;Value:FILETIME);
  function FileTimeToDateTime(FileTime:TFileTime):TDateTime;
  var
    ModifiedTime: TFileTime;
    SystemTime: TSystemTime;
  begin
    Result := 0;
    if (FileTime.dwLowDateTime = 0) and (FileTime.dwHighDateTime = 0) then
      Exit;
    try
      FileTimeToLocalFileTime(FileTime, ModifiedTime);
      FileTimeToSystemTime(ModifiedTime, SystemTime);
      Result := SystemTimeToDateTime(SystemTime);
    except
      Result := 0;
   end;
end;
begin
  WriteProperty(PropertyName,FileTimeToDateTime(Value));
end;

(* Writes the beginning of a JSON object. *)
procedure JsonWriter.WriteStartObject;
begin
  InternalWriteStart(JsonTokenStartObject,JsonContainerTypeObject);
end;

(* Writes the property name of a name/value pair on a JSON object. *)
procedure JsonWriter.WritePropertyName(PropertyName:String);
begin
  CurrentPosition.PropertyName := PropertyName;
  AutoComplete(JsonTokenPropertyName);
end;

procedure JsonWriter.WriteValue(Value:OleVariant);
  function DataTimeToStr(Value:TDateTime):String;
  begin
    Result := Format('%sT%s',[
      FormatDateTime('yyyy-MM-dd',Value),
      FormatDateTime('HH:mm:ss',Value)]);
  end;
var
  ValueType:TVarType;
  I,H,L:Integer;
begin
  ValueType := TVarData(Value).VType;
  try
    case ValueType of
      VT_BSTR: WriteValue(String(Value));
      VT_BOOL: WriteValue(Boolean(Value));
      VT_NULL: WriteNull;
      VT_I8  : InternalWriteValue(JsonTokenInteger,VarToStr(Value),False);
      VT_I4  : InternalWriteValue(JsonTokenInteger,VarToStr(Value),False);
      VT_I2  : InternalWriteValue(JsonTokenInteger,VarToStr(Value),False);
      VT_I1  : InternalWriteValue(JsonTokenInteger,VarToStr(Value),False);
      VT_UI8 : InternalWriteValue(JsonTokenInteger,VarToStr(Value),False);
      VT_UI4 : InternalWriteValue(JsonTokenInteger,VarToStr(Value),False);
      VT_UI2 : InternalWriteValue(JsonTokenInteger,VarToStr(Value),False);
      VT_UI1 : InternalWriteValue(JsonTokenInteger,VarToStr(Value),False);
      VT_DATE: WriteValue(DataTimeToStr(TVarData(Value).VDate));
      VT_ARRAY or VT_BSTR:
        begin
          WriteStartArray;
          L := VarArrayLowBound(Value,1);
          H := VarArrayHighBound(Value,1);
          for I := L to H do
          begin
            WriteValue(String(Value[I]));
          end;
          WriteEndArray;
        end;
    else
      WriteValue(VarToStr(Value));
    end;
  except
    on E:Exception do
    begin
      raise;
    end;
  end;
end;

procedure JsonWriter.WriteValue(Value:Integer);
begin
  InternalWriteValue(JsonTokenInteger);
end;

procedure JsonWriter.WriteValue(Value:Boolean);
begin
  InternalWriteValue(JsonTokenBoolean);
end;

procedure JsonWriter.WriteValue(Value:String);
begin
  InternalWriteValue(JsonTokenString);
end;

procedure JsonWriter.WriteValue(Value:PWideChar);
begin
  if Value <> nil then
    WriteValue(String(Value))
  else
    WriteNull;
end;

procedure JsonWriter.WriteStartArray;
begin
  InternalWriteStart(JsonTokenStartArray,JsonContainerTypeArray);
end;

procedure JsonWriter.WriteEndInternal(Container:JsonContainerType);
begin
  case Container of
    JsonContainerTypeObject: WriteEndObject;
    JsonContainerTypeArray : WriteEndArray;
    JsonContainerTypeConstructor: WriteEndConstructor;
  end;
end;

procedure JsonWriter.InternalWriteValue(Token:JsonToken;Value:String;
  Quote:Boolean);
begin
  UpdateScopeWithFinishedValue;
  AutoComplete(Token);
end;

procedure JsonWriter.WriteStartConstructor(ClassName: String);
begin
  InternalWriteStart(JsonTokenStartConstructor,JsonContainerTypeConstructor);
end;

procedure JsonWriter.WriteProperty(PropertyName:String;Value:PBStrList;const Count:Integer);
var
  I:Integer;
begin
  WritePropertyName(PropertyName);
  if (Value <> nil) and (Count > 0) then
  begin
    WriteStartArray;
    for I := 0 to Count-1 do
    begin
      WriteValue(Value[I]);
    end;
    WriteEnd;
    Exit;
  end;
  WriteNull;
end;

{ JsonTextWriter }

constructor JsonTextWriter.Create(Writer: TextWriter);
begin
  inherited Create;
  Assert(Writer <> nil);
  Self.Writer := Writer;
  Indentation := 2;
end;

procedure JsonWriter.InternalWriteStart(Token:JsonToken;Container:JsonContainerType);
begin
  UpdateScopeWithFinishedValue;
  AutoComplete(Token);
  Push(Container);
end;

procedure JsonTextWriter.InternalWriteValue(Token:JsonToken;
  Value:String;Quote:Boolean);
begin
  inherited;
  WriteEscapedString(Value,Quote);
end;

procedure JsonTextWriter.WriteEndInternal(Token:JsonToken);
begin
  case Token of
    JsonTokenEndObject      : Writer.Write('}');
    JsonTokenEndArray       : Writer.Write(']');
    JsonTokenEndConstructor : Writer.Write(')');
  end;
end;

(* Writes the beginning of a JSON object. *)
procedure JsonTextWriter.WriteEscapedString(Value:String;Quote:Boolean);
begin
  if Quote then Writer.Write('"');
  Writer.Write(Value);
  if Quote then Writer.Write('"');
end;

procedure JsonTextWriter.WriteIndent;
var
  IndentString:String;
  I,Count:Integer;
begin
  Count := Top*Indentation;
  IndentString := '';
  for I := 0 to Count - 1 do
  begin
    IndentString := IndentString + ' ';
  end;
  Writer.WriteLine;
  Writer.Write(IndentString);
end;

procedure JsonTextWriter.WriteIndentSpace;
begin
  Writer.Write(' ');
end;

procedure JsonTextWriter.WritePropertyName(PropertyName:String);
begin
  inherited;
  WriteEscapedString(PropertyName,True);
  Writer.Write(':');
end;

procedure JsonTextWriter.WriteRaw(Value:String);
begin
  inherited;
  Writer.Write(Value);
end;

procedure JsonTextWriter.WriteStartArray;
begin
  inherited;
  Writer.Write('[');
end;

procedure JsonTextWriter.WriteStartConstructor(ClassName:String);
begin
  inherited;
  Writer.Write('new ');
  Writer.Write(ClassName);
  Writer.Write('(');
end;

procedure JsonTextWriter.WriteStartObject;
begin
  inherited;
  Writer.Write('{');
end;

procedure JsonTextWriter.WriteValue(Value:Integer);
begin
  inherited;
  Writer.Write(Format('%d',[Value]));
end;

procedure JsonTextWriter.WriteValue(Value:Boolean);
begin
  inherited;
  if Value then
    Writer.Write('true')
  else
    Writer.Write('false');
end;

procedure JsonTextWriter.WriteValue(Value:String);
begin
  inherited;
  WriteEscapedString(Value,True);
end;

procedure JsonTextWriter.WriteValueDelimiter;
begin
  Writer.Write(',');
end;

{ ConsoleTextWriter }

constructor ConsoleTextWriter.Create;
begin
  inherited;
end;

procedure ConsoleTextWriter.Write(Value:String);
begin
  System.Write(Value);
end;

procedure ConsoleTextWriter.WriteLine;
begin
  Writeln;
end;

{ JsonPosition }

constructor JsonPosition.Create;
begin
  Container := JsonContainerTypeNone;
  Position := -1;
  HasIndex := False;
  PropertyName := '';
end;

constructor JsonPosition.Create(Container:JsonContainerType);
begin
  Self.Container := Container;
  Position := -1;
  HasIndex := TypeHasIndex(Container);
  PropertyName := '';
end;

class function JsonPosition.TypeHasIndex(Container:JsonContainerType): Boolean;
begin
  Result := (Container = JsonContainerTypeArray) or
            (Container = JsonContainerTypeConstructor);
end;

var
  I:Integer;

procedure Move(Source:TJsonStateArray;var Target:TJsonStateArray);
var
  I:Integer;
begin
  for I := 0 to 9 do
  begin
    Target[I] := Source[I];
  end;
end;

{ TextWriter }

constructor TextWriter.Create;
begin
end;

initialization
  StateArrayTemplate[0,0] := JsonStateError;
  StateArrayTemplate[0,1] := JsonStateError;
  StateArrayTemplate[0,2] := JsonStateError;
  StateArrayTemplate[0,3] := JsonStateError;
  StateArrayTemplate[0,4] := JsonStateError;
  StateArrayTemplate[0,5] := JsonStateError;
  StateArrayTemplate[0,6] := JsonStateError;
  StateArrayTemplate[0,7] := JsonStateError;
  StateArrayTemplate[0,8] := JsonStateError;
  StateArrayTemplate[0,9] := JsonStateError;
  StateArrayTemplate[1,0] := JsonStateObjectStart;
  StateArrayTemplate[1,1] := JsonStateObjectStart;
  StateArrayTemplate[1,2] := JsonStateError;
  StateArrayTemplate[1,3] := JsonStateError;
  StateArrayTemplate[1,4] := JsonStateObjectStart;
  StateArrayTemplate[1,5] := JsonStateObjectStart;
  StateArrayTemplate[1,6] := JsonStateObjectStart;
  StateArrayTemplate[1,7] := JsonStateObjectStart;
  StateArrayTemplate[1,8] := JsonStateError;
  StateArrayTemplate[1,9] := JsonStateError;
  StateArrayTemplate[2,0] := JsonStateArrayStart;
  StateArrayTemplate[2,1] := JsonStateArrayStart;
  StateArrayTemplate[2,2] := JsonStateError;
  StateArrayTemplate[2,3] := JsonStateError;
  StateArrayTemplate[2,4] := JsonStateArrayStart;
  StateArrayTemplate[2,5] := JsonStateArrayStart;
  StateArrayTemplate[2,6] := JsonStateArrayStart;
  StateArrayTemplate[2,7] := JsonStateArrayStart;
  StateArrayTemplate[2,8] := JsonStateError;
  StateArrayTemplate[2,9] := JsonStateError;
  StateArrayTemplate[3,0] := JsonStateConstructorStart;
  StateArrayTemplate[3,1] := JsonStateConstructorStart;
  StateArrayTemplate[3,2] := JsonStateError;
  StateArrayTemplate[3,3] := JsonStateError;
  StateArrayTemplate[3,4] := JsonStateConstructorStart;
  StateArrayTemplate[3,5] := JsonStateConstructorStart;
  StateArrayTemplate[3,6] := JsonStateConstructorStart;
  StateArrayTemplate[3,7] := JsonStateConstructorStart;
  StateArrayTemplate[3,8] := JsonStateError;
  StateArrayTemplate[3,9] := JsonStateError;
  StateArrayTemplate[4,0] := JsonStateProperty;
  StateArrayTemplate[4,1] := JsonStateError;
  StateArrayTemplate[4,2] := JsonStateProperty;
  StateArrayTemplate[4,3] := JsonStateProperty;
  StateArrayTemplate[4,4] := JsonStateError;
  StateArrayTemplate[4,5] := JsonStateError;
  StateArrayTemplate[4,6] := JsonStateError;
  StateArrayTemplate[4,7] := JsonStateError;
  StateArrayTemplate[4,8] := JsonStateError;
  StateArrayTemplate[4,9] := JsonStateError;
  StateArrayTemplate[5,0] := JsonStateStart;
  StateArrayTemplate[5,1] := JsonStateProperty;
  StateArrayTemplate[5,2] := JsonStateObjectStart;
  StateArrayTemplate[5,3] := JsonStateObject;
  StateArrayTemplate[5,4] := JsonStateArrayStart;
  StateArrayTemplate[5,5] := JsonStateArray;
  StateArrayTemplate[5,6] := JsonStateConstructor;
  StateArrayTemplate[5,7] := JsonStateConstructor;
  StateArrayTemplate[5,8] := JsonStateError;
  StateArrayTemplate[5,9] := JsonStateError;
  StateArrayTemplate[6,0] := JsonStateStart;
  StateArrayTemplate[6,1] := JsonStateProperty;
  StateArrayTemplate[6,2] := JsonStateObjectStart;
  StateArrayTemplate[6,3] := JsonStateObject;
  StateArrayTemplate[6,4] := JsonStateArrayStart;
  StateArrayTemplate[6,5] := JsonStateArray;
  StateArrayTemplate[6,6] := JsonStateConstructor;
  StateArrayTemplate[6,7] := JsonStateConstructor;
  StateArrayTemplate[6,8] := JsonStateError;
  StateArrayTemplate[6,9] := JsonStateError;
  StateArrayTemplate[7,0] := JsonStateStart;
  StateArrayTemplate[7,1] := JsonStateObject;
  StateArrayTemplate[7,2] := JsonStateError;
  StateArrayTemplate[7,3] := JsonStateError;
  StateArrayTemplate[7,4] := JsonStateArray;
  StateArrayTemplate[7,5] := JsonStateArray;
  StateArrayTemplate[7,6] := JsonStateConstructor;
  StateArrayTemplate[7,7] := JsonStateConstructor;
  StateArrayTemplate[7,8] := JsonStateError;
  StateArrayTemplate[7,9] := JsonStateError;

  SetLength(StateArray,8 + 10);
  for I := 0 to 7 do
  begin
    Move(StateArrayTemplate[I],StateArray[I]);
  end;
  Move(StateArrayTemplate[7],StateArray[ 8]);
  Move(StateArrayTemplate[7],StateArray[ 9]);
  Move(StateArrayTemplate[7],StateArray[10]);
  Move(StateArrayTemplate[7],StateArray[11]);
  Move(StateArrayTemplate[7],StateArray[12]);
  Move(StateArrayTemplate[0],StateArray[13]);
  Move(StateArrayTemplate[0],StateArray[14]);
  Move(StateArrayTemplate[0],StateArray[15]);
  Move(StateArrayTemplate[7],StateArray[16]);
  Move(StateArrayTemplate[7],StateArray[17]);
end.
