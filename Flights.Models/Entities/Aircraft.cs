using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations.Schema;
using Flights.Models.Entities.Base;

namespace Flights.Models.Entities;

public partial class Aircraft : BaseEntity<string?>
{
    [NotMapped]
    public override string? Key => AircraftCode;

    /// <summary>
    /// Aircraft code, IATA
    /// </summary>
    public string? AircraftCode { get; set; }

    /// <summary>
    /// Aircraft model
    /// </summary>
    public string? Model { get; set; }

    /// <summary>
    /// Maximal flying distance, km
    /// </summary>
    public int? Range { get; set; }
}
