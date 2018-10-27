using System;
using Newtonsoft.Json;

namespace Axh.Fit.Endomondo.Models
{
    /// <summary>
    /// An Endomondo workout.
    /// </summary>
    public class Workout
    {
        /// <summary>
        /// Gets or sets the identifier.
        /// </summary>
        /// <value>
        /// The identifier.
        /// </value>
        public int Id { get; set; }

        /// <summary>
        /// Gets or sets the local start time.
        /// </summary>
        /// <value>
        /// The local start time.
        /// </value>
        [JsonProperty(PropertyName = "local_start_time")]
        public DateTime LocalStartTime { get; set; }
    }
}