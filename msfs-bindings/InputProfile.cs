using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace msfs_bindings
{
    public class InputProfile
    {
        public Version Version { get; set; } = null!;
        public FriendlyName  FriendlyName { get; set; } = null!;
        public Device Device { get; set; } = null!;
    }
    // NOTE: Generated code may require at least .NET Framework 4.5 or .NET Core/Standard 2.0.
  
    [System.SerializableAttribute()]
    [System.ComponentModel.DesignerCategoryAttribute("code")]
    [System.Xml.Serialization.XmlTypeAttribute(AnonymousType = true)]
    [System.Xml.Serialization.XmlRootAttribute(Namespace = "", IsNullable = false)]
    public partial class Version
    {
        [System.Xml.Serialization.XmlAttributeAttribute()]
        public int Num { get; set; }
    }

  
    [System.SerializableAttribute()]
    [System.ComponentModel.DesignerCategoryAttribute("code")]
    [System.Xml.Serialization.XmlTypeAttribute(AnonymousType = true)]
    [System.Xml.Serialization.XmlRootAttribute(Namespace = "", IsNullable = false)]
    public partial class FriendlyName
    {
     
        [System.Xml.Serialization.XmlAttributeAttribute()]
        public int PlatformAvailability { get; set; }
      
        [System.Xml.Serialization.XmlTextAttribute()]
        public string Value { get; set; } = null!;
    }

    
  
    [System.SerializableAttribute()]
    [System.ComponentModel.DesignerCategoryAttribute("code")]
    [System.Xml.Serialization.XmlTypeAttribute(AnonymousType = true)]
    [System.Xml.Serialization.XmlRootAttribute(Namespace = "", IsNullable = false)]
    public partial class Device
    {
        [System.Xml.Serialization.XmlElementAttribute("Axes")]
        public List<DeviceAxis> Axes { get; set; } = null!;

        [System.Xml.Serialization.XmlElementAttribute("Context")]
        public List<DeviceContext> Context { get; set; } = null!;

        [System.Xml.Serialization.XmlAttributeAttribute()]
        public string DeviceName { get; set; } = null!;

        [System.Xml.Serialization.XmlAttributeAttribute()]
        public string GUID { get; set; } = null!;

        [System.Xml.Serialization.XmlAttributeAttribute()]
        public int ProductID { get; set; }
      
        [System.Xml.Serialization.XmlAttributeAttribute()]
        public int CompositeID { get; set; }
    }

  
    [System.SerializableAttribute()]
    [System.ComponentModel.DesignerCategoryAttribute("code")]
    [System.Xml.Serialization.XmlTypeAttribute(AnonymousType = true)]
    public partial class DeviceAxis
    {
      
        [System.Xml.Serialization.XmlAttributeAttribute()]
        public string AxisName { get; set; } = null!;

        [System.Xml.Serialization.XmlAttributeAttribute()]
        public int AxisSensitivy { get; set; }
      
        [System.Xml.Serialization.XmlAttributeAttribute()]
        public int AxisSensitivyMinus { get; set; }
      
        [System.Xml.Serialization.XmlAttributeAttribute()]
        public int AxisNeutral { get; set; }
      
        [System.Xml.Serialization.XmlAttributeAttribute()]
        public int AxisDeadZone { get; set; }
      
        [System.Xml.Serialization.XmlAttributeAttribute()]
        public int AxisOutDeadZone { get; set; }
      
        [System.Xml.Serialization.XmlAttributeAttribute()]
        public int AxisResponseRate { get; set; }
      
    }



  
    [System.SerializableAttribute()]
    [System.ComponentModel.DesignerCategoryAttribute("code")]
    [System.Xml.Serialization.XmlTypeAttribute(AnonymousType = true)]
    public partial class DeviceContext
    {
      
        [System.Xml.Serialization.XmlElementAttribute("Action")]
        public List<DeviceContextAction> Actions { get; set; } = null!;

        [System.Xml.Serialization.XmlAttributeAttribute()]
        public string ContextName { get; set; } = null!;
    }

  
    [System.SerializableAttribute()]
    [System.ComponentModel.DesignerCategoryAttribute("code")]
    [System.Xml.Serialization.XmlTypeAttribute(AnonymousType = true)]
    public partial class DeviceContextAction
    {
      
        [System.Xml.Serialization.XmlArrayItemAttribute("KEY", IsNullable = false)]
        public List<DeviceContextActionKEY> Primary { get; set; } = null!;

        [System.Xml.Serialization.XmlArrayItemAttribute("KEY", IsNullable = false)]
        public List<DeviceContextActionKEY> Secondary { get; set; } = null!;

        [System.Xml.Serialization.XmlAttributeAttribute()]
        public string ActionName { get; set; } = null!;

        [System.Xml.Serialization.XmlAttributeAttribute()]
        public int Flag { get; set; }
    }

  
    [System.SerializableAttribute()]
    [System.ComponentModel.DesignerCategoryAttribute("code")]
    [System.Xml.Serialization.XmlTypeAttribute(AnonymousType = true)]
    public partial class DeviceContextActionKEY
    {
        [System.Xml.Serialization.XmlAttributeAttribute()]
        public string Information { get; set; } = null!;

        [System.Xml.Serialization.XmlTextAttribute()]
        public int Value { get; set; }
    }

 


}
