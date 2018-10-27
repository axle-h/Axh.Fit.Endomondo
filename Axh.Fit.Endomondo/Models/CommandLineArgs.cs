using System.Collections.Generic;
using CommandLine;

namespace Axh.Fit.Endomondo.Models
{
    /// <summary>
    /// The command line arguments.
    /// </summary>
    public class CommandLineArgs
    {
        /// <summary>
        /// Gets or sets the user token.
        /// </summary>
        /// <value>
        /// The user token.
        /// </value>
        [Option('u', "user token", Required = true, HelpText = "Endomondo USER_TOKEN cookie value.")]
        public string UserToken { get; set; }

        /// <summary>
        /// Gets or sets a value indicating whether [GPX file format].
        /// </summary>
        /// <value>
        ///   <c>true</c> if [GPX file format]; otherwise, <c>false</c>.
        /// </value>
        [Option('g', "gpx", HelpText = "Download GPX format (recommended for Strava).")]
        public bool Gpx { get; set; }

        /// <summary>
        /// Gets or sets a value indicating whether [TCX file format].
        /// </summary>
        /// <value>
        ///   <c>true</c> if [TCX file format]; otherwise, <c>false</c>.
        /// </value>
        [Option('t', "tcx", HelpText = "Download TCX format.")]
        public bool Tcx { get; set; }

        /// <summary>
        /// Gets the formats.
        /// </summary>
        /// <returns></returns>
        public IEnumerable<string> GetFormats()
        {
            if (Gpx)
            {
                yield return "gpx";
            }

            if (Tcx)
            {
                yield return "tcx";
            }
        }

    }
}
