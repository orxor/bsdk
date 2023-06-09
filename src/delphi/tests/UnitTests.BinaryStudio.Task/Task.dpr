program Task;

{$APPTYPE CONSOLE}

uses
  SysUtils,
  TypInfo,
  Forms,
  Json in '..\..\core\Json.pas';

const
  {$IF DEFINED(VER140) OR DEFINED(VER150)}
  TypeKindNames:array[TTypeKind] of String = ('tkUnknown','tkInteger','tkChar',
    'tkEnumeration','tkFloat','tkString','tkSet','tkClass','tkMethod','tkWChar',
    'tkLString','tkWString','tkVariant','tkArray','tkRecord','tkInterface',
    'tkInt64','tkDynArray');
  {$ELSE}
  TypeKindNames:array[TTypeKind] of String = ('tkUnknown','tkInteger','tkChar',
    'tkEnumeration','tkFloat','tkString','tkSet','tkClass','tkMethod','tkWChar',
    'tkLString','tkWString','tkVariant','tkArray','tkRecord','tkInterface',
    'tkInt64','tkDynArray','tkUString','tkClassRef','tkPointer','tkProcedure',
    'tkMRecord');
  {$IFEND}

procedure Write(Writer:JsonWriter;PropInfo:PPropInfo);overload;
begin
  Writer.WriteStartObject;
    Writer.WriteProperty('Name',PropInfo.Name);
    Writer.WriteProperty('NameIndex',PropInfo.NameIndex);
    Writer.WriteProperty('Index',PropInfo.Index);
    Writer.WriteProperty('Default',PropInfo.Default);
    if PropInfo.PropType <> nil then
    begin
      Writer.WriteProperty('PropType',PropInfo.PropType^.Name);
    end;
    Writer.WriteProperty('CanRead',PropInfo.GetProc <> nil);
    Writer.WriteProperty('CanWrite',PropInfo.SetProc <> nil);
  Writer.WriteEnd;
end;

procedure Write(Writer:JsonWriter;TypeInfo:PTypeInfo;ClassType:TClass);overload;
var
  I,Count:Integer;
  PropList:PPropList;
  MthdList:PTypeData;
  TypeData:PTypeData;
begin
  Writer.WriteStartObject;
    Writer.WriteProperty('Name',TypeInfo.Name);
    Writer.WriteProperty('Kind',TypeKindNames[TypeInfo.Kind]);
    Writer.WritePropertyName('Properties');
    Count := GetPropList(TypeInfo,tkProperties,nil);
    GetMem(PropList,Count*SizeOf(PPropInfo));
    Writer.WriteProperty('Count',Count);
    Writer.WritePropertyName('{Self}');
      Writer.WriteStartArray;
      try
        for I := 0 to Pred(Count) do
        begin
          Write(Writer,PropList^[I]);
        end;
      finally
        FreeMem(PropList,Count*SizeOf(PPropInfo))
      end;
      Writer.WriteEnd;
    TypeData := GetTypeData(TypeInfo);
    if TypeData <> nil then
    begin

    end;
  Writer.WriteEnd;
end;

var
  TypeInfo:PTypeInfo;
  Writer:JsonWriter;
begin
  Writer := JsonTextWriter.Create(ConsoleTextWriter.Create);
  try
    TypeInfo := PTypeInfo(TForm.ClassInfo);
    if TypeInfo <> nil then
    begin
      Write(Writer,TypeInfo,TForm);
    end;
    Writer.WriteEnd;
  finally
    FreeAndNil(Writer);
  end;
end.
