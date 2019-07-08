using System;

namespace Mic.Repository.Utils
{
    public abstract class Duration
    {
        /// <summary>
        /// Abstract method of getting duration(ms) of audio or vedio
        /// </summary>
        /// <param name="filePath">audio/vedio's path</param>
        /// <returns>Duration in original format, duration in milliseconds</returns>
        public abstract Tuple<string, long> GetDuration(string filePath);

        /// <summary>
        /// Convert format of "00:10:16" and "00:00:19.82" into milliseconds
        /// </summary>
        /// <param name="formatTime"></param>
        /// <returns>Time in milliseconds</returns>
        public long GetTimeInMillisecond(string formatTime)
        {
            double totalMilliSecends = 0;

            if (!string.IsNullOrEmpty(formatTime))
            {
                string[] timeParts = formatTime.Split(':');
                totalMilliSecends = Convert.ToInt16(timeParts[0]) * 60 * 60 * 1000
                    + Convert.ToInt16(timeParts[1]) * 60 * 1000
                    + Math.Round(double.Parse(timeParts[2]) * 1000);
            }

            return (long)totalMilliSecends;
        }
    }
}
