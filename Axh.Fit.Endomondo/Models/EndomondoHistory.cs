using System.Collections.Generic;

namespace Axh.Fit.Endomondo.Models
{
    /// <summary>
    /// A history response from the rest v1 Endomondo API.
    /// </summary>
    public class EndomondoHistory
    {
        /// <summary>
        /// Gets or sets the data.
        /// </summary>
        /// <value>
        /// The data.
        /// </value>
        public ICollection<EndomondoWorkout> Data { get; set; }
    }
}