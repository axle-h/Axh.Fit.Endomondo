using CommandLine;
using CommandLine.Text;

namespace Axh.Fit.Endomondo.Models
{
    /// <summary>
    /// The command line arguments.
    /// </summary>
    public class CommandLineArgs
    {
        private const string FileFormatSet = "file format";
        
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
        [Option('g', "gpx", DefaultValue = true, HelpText = "Download GPX format (recommended for Strava).", MutuallyExclusiveSet = FileFormatSet)]
        public bool GpxFileFormat { get; set; }

        /// <summary>
        /// Gets or sets a value indicating whether [TCX file format].
        /// </summary>
        /// <value>
        ///   <c>true</c> if [TCX file format]; otherwise, <c>false</c>.
        /// </value>
        [Option('t', "tcx", HelpText = "Download TCX format.", MutuallyExclusiveSet = FileFormatSet)]
        public bool TcxFileFormat { get; set; }

        /// <summary>
        /// Gets the usage.
        /// </summary>
        /// <returns></returns>
        [HelpOption]
        public string GetUsage() => HelpText.AutoBuild(this, current => HelpText.DefaultParsingErrorsHandler(this, current));

    }
}
