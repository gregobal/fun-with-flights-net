using System.ComponentModel.DataAnnotations.Schema;

[Table("aircrafts_data")]
public class Aircraft
{
    [Key] [Column("aircraft_code")] public string Code { get; set; } = "000";
    [Column("model", TypeName = "jsonb")] public string Model { get; set; } = String.Empty;
    [Column("range")] public int Range { get; set; }
}