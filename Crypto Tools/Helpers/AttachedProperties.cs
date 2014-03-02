using System.Windows;
using System.Windows.Media;

namespace CryptoTools.Helpers
{
    /// <summary>
    /// http://xiu.shoeke.com/2010/07/16/wpf-image-buttons/
    /// http://www.hardcodet.net/2009/01/create-wpf-image-button-through-attached-properties
    /// </summary>
    public class AttachedProperties
    {
        #region Image dependency property

        /// <summary>
        /// An attached dependency property which provides an
        /// <see cref="ImageSource" /> for arbitrary WPF elements.
        /// </summary>
        public static readonly DependencyProperty ImageProperty;

        /// <summary>
        /// Gets the <see cref="ImageProperty"/> for a given
        /// <see cref="DependencyObject"/>, which provides an
        /// <see cref="ImageSource" /> for arbitrary WPF elements.
        /// </summary>
        public static ImageSource GetImage(DependencyObject obj)
        {
            return (ImageSource) obj.GetValue(ImageProperty);
        }

        /// <summary>
        /// Sets the attached <see cref="ImageProperty"/> for a given
        /// <see cref="DependencyObject"/>, which provides an
        /// <see cref="ImageSource" /> for arbitrary WPF elements.
        /// </summary>
        public static void SetImage(DependencyObject obj, ImageSource value)
        {
            obj.SetValue(ImageProperty, value);
        }

        #endregion


        //#region Image dependency property

        ///// <summary>
        ///// An attached dependency property which provides an
        ///// <see cref="ImageSource" /> for arbitrary WPF elements.
        ///// </summary>
        //public static readonly DependencyProperty ImageSizeProperty;

        ///// <summary>
        ///// Gets the <see cref="ImageProperty"/> for a given
        ///// <see cref="DependencyObject"/>, which provides an
        ///// <see cref="ImageSource" /> for arbitrary WPF elements.
        ///// </summary>
        //public static int GetImageSize(DependencyObject obj)
        //{
        //    return (int)obj.GetValue(ImageProperty);
        //}

        ///// <summary>
        ///// Sets the attached <see cref="ImageProperty"/> for a given
        ///// <see cref="DependencyObject"/>, which provides an
        ///// <see cref="ImageSource" /> for arbitrary WPF elements.
        ///// </summary>
        //public static void SetImageSize(DependencyObject obj, int value)
        //{
        //    obj.SetValue(ImageSizeProperty, value);
        //}

        //#endregion
        static AttachedProperties()
        {
            //register attached dependency property
            var metadata = new FrameworkPropertyMetadata((ImageSource) null);
            ImageProperty = DependencyProperty.RegisterAttached("Image",
                                                                typeof (ImageSource),
                                                                typeof (AttachedProperties), metadata);
            //ImageSizeProperty = DependencyProperty.RegisterAttached("ImageSize",
            //                                        typeof(int),
            //                                        typeof(AttachedProperties), metadata);
            
        }
    }
}