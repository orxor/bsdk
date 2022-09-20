using System.ComponentModel;
using System.ComponentModel.DataAnnotations;

namespace BinaryStudio.PortableExecutable.Win32
    {
    public enum IMAGE_RESOURCE_LEVEL
        {
        [Display(Name="Type")]     LEVEL_TYPE,
        [Display(Name="Name")]     LEVEL_NAME,
        [Display(Name="Language")] LEVEL_LANGUAGE
        }
    }