using Application.Common.Interfaces;

namespace Infrastructure.Services;

/// <summary>
/// Implementation for the <see cref="IDateTime"/> interface
/// </summary>
public class DateTimeService : IDateTime
{
    /// <summary>
    /// Returns  current date time: 03/22/2023 23:59:59.9999999
    /// </summary>
    public DateTime Now => DateTime.Now;
}