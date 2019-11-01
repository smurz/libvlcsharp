﻿using System;
using Windows.UI.Xaml;
using Windows.UI.Xaml.Controls;

namespace LibVLCSharp.Uno
{
    /// <summary>
    /// Represents an object that uses a <see cref="Shared.MediaPlayer"/> to render audio and video to the display
    /// </summary>
    public partial class MediaPlayerElement : ContentControl
    {
        /// <summary>
        /// Occurs when the <see cref="MediaPlayerElement"/> is fully loaded
        /// </summary>
        public event EventHandler<InitializedEventArgs>? Initialized;

        /// <summary>
        /// Initializes a new instance of the <see cref="MediaPlayerElement"/> class
        /// </summary>
        public MediaPlayerElement()
        {
            DefaultStyleKey = typeof(MediaPlayerElement);
        }

        /// <summary>
        /// Identifies the <see cref="AreTransportControlsEnabled"/> dependency property
        /// </summary>
        public static readonly DependencyProperty AreTransportControlsEnabledProperty = DependencyProperty.Register(
            nameof(AreTransportControlsEnabled), typeof(bool), typeof(MediaPlayerElement), new PropertyMetadata(true));
        /// <summary>
        /// Gets or sets a value that determines whether the standard transport controls are enabled
        /// </summary>
        public bool AreTransportControlsEnabled
        {
            get => (bool)GetValue(AreTransportControlsEnabledProperty);
            set => SetValue(AreTransportControlsEnabledProperty, value);
        }

        /// <summary>
        /// Identifies the <see cref="MediaPlayer"/> dependency property
        /// </summary>
        public static readonly DependencyProperty MediaPlayerProperty = DependencyProperty.Register(nameof(MediaPlayer), typeof(Shared.MediaPlayer),
            typeof(MediaPlayerElement), new PropertyMetadata(null, UpdateTransportControlsMediaPlayer));
        /// <summary>
        /// Gets the <see cref="Shared.MediaPlayer"/> instance
        /// </summary>
        public Shared.MediaPlayer? MediaPlayer
        {
            get => (Shared.MediaPlayer)GetValue(MediaPlayerProperty);
            set => SetValue(MediaPlayerProperty, value);
        }

        /// <summary>
        /// Identifies the <see cref="TransportControls"/> dependency property
        /// </summary>
        private static readonly DependencyProperty TransportControlsProperty = DependencyProperty.Register(nameof(TransportControls),
            typeof(MediaTransportControls), typeof(MediaPlayerElement),
            new PropertyMetadata(new MediaTransportControls(), UpdateTransportControlsMediaPlayer));
        /// <summary>
        /// Gets or sets the transport controls for the media
        /// </summary>
        public MediaTransportControls? TransportControls
        {
            get => (MediaTransportControls)GetValue(TransportControlsProperty);
            set => SetValue(TransportControlsProperty, value);
        }

        private VideoView? VideoView { get; set; }

        /// <summary>
        /// Invoked whenever application code or internal processes (such as a rebuilding layout pass) call <see cref="Control.ApplyTemplate"/>.
        /// In simplest terms, this means the method is called just before a UI element displays in your app
        /// </summary>
        protected override void OnApplyTemplate()
        {
            base.OnApplyTemplate();
            VideoView = (VideoView)GetTemplateChild("VideoView");
            VideoView.Initialized += (sender, e) => Initialized?.Invoke(this, e);
            if (GetTemplateChild("ContentPresenter") is UIElement contentGrid)
            {
                contentGrid.PointerEntered += OnPointerMoved;
                contentGrid.PointerMoved += OnPointerMoved;
                contentGrid.Tapped += OnPointerMoved;
            }
            UpdateTransportControlsMediaPlayer();
        }

        private static void UpdateTransportControlsMediaPlayer(DependencyObject dependencyObject, DependencyPropertyChangedEventArgs args)
        {
            ((MediaPlayerElement)dependencyObject).UpdateTransportControlsMediaPlayer();
        }

        private void OnPointerMoved(object sender, RoutedEventArgs e)
        {
            TransportControls?.Show();
        }

        private void UpdateTransportControlsMediaPlayer()
        {
            var transportControls = TransportControls;
            if (transportControls != null)
            {
                transportControls.MediaPlayer = MediaPlayer;
            }
        }
    }
}