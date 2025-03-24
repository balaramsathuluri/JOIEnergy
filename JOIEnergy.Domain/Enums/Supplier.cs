using System.Runtime.Serialization;
using System.Text.Json.Serialization;

namespace JOIEnergy.Domain.Enums
{
    [JsonConverter(typeof(JsonStringEnumConverter))]
    public enum Supplier
    {
        [EnumMember(Value = "Dr Evil's Dark Energy")]
        DrEvilsDarkEnergy,

        [EnumMember(Value = "The Green Eco")]
        TheGreenEco,

        [EnumMember(Value = "Power for Everyone")]
        PowerForEveryone
    }
}
