using Blazor.CssBundler.Models;
using Blazor.CssBundler.Models.Bundle;
using Blazor.CssBundler.Models.Settings;
using ExCSS;
using System;
using System.Collections.Generic;
using System.Diagnostics;
using System.IO;
using System.Threading.Tasks;

namespace Blazor.CssBundler.Bundle
{
    abstract class AutoBundler<TSettings, TBundle> : IWatcher
        where TSettings : BaseSettings
        where TBundle : BundleInfoBase
    {
        protected TSettings Settings { get; set; }

        /// <summary>
        /// Using for watching changed, created and etc. css files
        /// </summary>
        protected FileSystemWatcher FileWatcher { get; private set; }

        /// <summary>
        /// Css style parser
        /// </summary>
        protected StylesheetParser CssParser { get; private set; }

        /// <summary>
        /// Used for determine time to build css bundle
        /// </summary>
        protected Stopwatch BuildStopWatch { get; private set; }

        public delegate void BuildEndHandler(TBundle buildInfo);
        public delegate void BuildErrorHandler(TBundle buildInfo);
        public delegate void BuildStartHandler();

        /// <summary>
        /// The event is fired when build ending
        /// </summary>
        public event BuildEndHandler BundlingEnd;

        /// <summary>
        /// The event is fired when build starting
        /// </summary>
        public event BuildStartHandler BundlingStart;

        /// <summary>
        /// The event is fired when build error
        /// </summary>
        public event BuildErrorHandler BundlingError;

        public AutoBundler(TSettings settings)
        {
            Settings = settings;
            CssParser = new StylesheetParser();

            // Creating build time watcher.
            BuildStopWatch = new Stopwatch();

            // Creating file watcher.
            FileWatcher = new FileSystemWatcher(Settings.ProjectDirectory, Settings.CssRazorSearchPattern);
            FileWatcher.IncludeSubdirectories = true;

            // Add event handlers.
            FileWatcher.Created += FileWatcherCreated;
            FileWatcher.Deleted += FileWatcherDeleted;
            FileWatcher.Changed += FileWatcherChanged;
            FileWatcher.Renamed += FileWatcherRenamed;
        }

        protected void OnBundlingStart()
        {
            BundlingStart?.Invoke();
        }

        protected void OnBundlingEnd(TBundle buildInfo)
        {
            BundlingEnd?.Invoke(buildInfo);
        }

        protected void OnBundlingError(TBundle buildInfo)
        {
            BundlingError?.Invoke(buildInfo);
        }

        public async Task StartWatchingAsync()
        {
            // First building.
            await Task.Run(() =>
            {
                Build();
            });

            // Enabling event handlers.
            FileWatcher.EnableRaisingEvents = true;
        }

        public void StopWatching()
        {
            // Disabling event handlers.
            FileWatcher.EnableRaisingEvents = false;
        }

        /// <summary>
        /// Build css files
        /// </summary>
        public abstract Task<TBundle> Build();

        protected virtual void FileWatcherRenamed(object sender, RenamedEventArgs e)
        {
            // 'visual studio code' generating tmp file and system watcher detecting this file as "Old file name"
            // we dont allow to rebuild if tmp generated because tmp files is garbage
            if (!e.FullPath.ToLower().EndsWith("tmp"))
            {
                Build();
            }
        }

        protected virtual void FileWatcherChanged(object sender, FileSystemEventArgs e)
        {         
            Build();
        }

        protected virtual void FileWatcherDeleted(object sender, FileSystemEventArgs e)
        {
            Build();
        }

        protected virtual void FileWatcherCreated(object sender, FileSystemEventArgs e)
        {
            Build();
        }
    }
}
