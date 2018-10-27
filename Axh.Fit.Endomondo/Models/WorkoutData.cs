using System;
using System.IO;

namespace Axh.Fit.Endomondo.Models
{
    /// <summary>
    /// An Endomondo workout data file.
    /// </summary>
    public class WorkoutData : IDisposable
    {
        /// <summary>
        /// Gets or sets the length of the stream in bytes.
        /// </summary>
        /// <value>
        /// The length of the stream in bytes.
        /// </value>
        public int Length { get; set; }

        /// <summary>
        /// Gets or sets the stream.
        /// </summary>
        /// <value>
        /// The stream.
        /// </value>
        public Stream Stream { get; set; }

        /// <summary>
        /// Performs application-defined tasks associated with freeing, releasing, or resetting unmanaged resources.
        /// </summary>
        public void Dispose() => Stream?.Dispose();
    }
}