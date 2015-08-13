using System;
using System.Drawing;
using System.IO;
using System.Reflection;
using System.Windows.Forms;

namespace Vurdalakov.Reflection
{
    /// <summary>
    /// This class is used to load embedded resources from assemblies.
    /// </summary>
    public class EmbeddedResources
    {
        private Assembly assembly;

        /// <summary>
        /// Initializes a new instance of the class with the currently executing assembly.
        /// </summary>
        public EmbeddedResources() : this(null)
        {
        }

        /// <summary>
        /// Initializes a new instance of the class with the specified assembly.
        /// </summary>
        /// <param name="assembly">Assembly to load embedded resources from. If <c>null</c>, then currently executing assembly is used.</param>
        public EmbeddedResources(Assembly assembly)
        {
            this.assembly = null == assembly ? Assembly.GetExecutingAssembly() : assembly;
        }

        /// <summary>
        /// Loads the specified embedded resource from the assembly.
        /// </summary>
        /// <param name="resourceName">The case-sensitive name of the embedded resource being requested.</param>
        /// <returns>A <see cref="System.IO.Stream"/> representing the embedded resource; <c>null</c> if no resources were specified during compilation, or if the resource is not visible to the caller.</returns>
        /// <remarks>
        /// <para>Include resource file into the project as "Embedded Resource". The <paramref name="resourceName"/> parameter is the name of the resource file as it is seen in the Solution Explorer.</para>
        /// <para>This methods throws an exception if embedded resource is not found, this is a design-time problem.</para>
        /// </remarks>
        /// <exception cref="System.ArgumentNullException">The <paramref name="resourceName"/> parameter is <c>null</c>.</exception>
        /// <exception cref="System.ArgumentException">The name parameter is an empty string ("").</exception>
        /// <exception cref="System.IO.FileLoadException">A file that was found could not be loaded.</exception>
        /// <exception cref="System.IO.FileNotFoundException"><paramref name="resourceName"/> was not found.</exception>
        /// <exception cref="System.BadImageFormatException">Not a valid assembly.</exception>
        public Stream GetResourceStream(String resourceName)
        {
            Stream resourceStream = null;

            String[] manifestResourceNames = assembly.GetManifestResourceNames();
            foreach (String manifestResourceName in manifestResourceNames)
            {
                if (manifestResourceName.EndsWith(resourceName))
                {
                    resourceStream = assembly.GetManifestResourceStream(manifestResourceName);
                    break;
                }
            }
            
            if (null == resourceStream)
            {
                throw new FileNotFoundException(String.Format("Resource not found: '{0}'", resourceName));
            }

            return resourceStream;
        }

        /// <summary>
        /// <para>Loads the specified embedded <see cref="System.Drawing.Bitmap"/> from the assembly.</para>
        /// <para>See <c>Remarks</c> and <c>Exceptions</c> in <see cref="GetResource"/> for more information.</para>
        /// </summary>
        /// <param name="resourceName">The case-sensitive name of the embedded resource being requested.</param>
        /// <returns>A <c>Bitmap</c> representing the embedded resource; <c>null</c> if no resources were specified during compilation, or if the resource is not visible to the caller.</returns>
        public Bitmap GetBitmap(String resourceName)
        {
            Bitmap bitmap = new Bitmap(GetResourceStream(resourceName));

            return bitmap;
        }

        /// <summary>
        /// <para>Loads the specified embedded <see cref="System.Drawing.Bitmap"/> from the assembly.</para>
        /// <para>See <c>Remarks</c> and <c>Exceptions</c> in <see cref="GetResource"/> for more information.</para>
        /// </summary>
        /// <param name="resourceName">The case-sensitive name of the embedded resource being requested.</param>
        /// <param name="assembly">Assembly to load embedded resources from. If <c>null</c>, then currently executing assembly is used.</param>
        /// <returns>A <c>Bitmap</c> representing the embedded resource; <c>null</c> if no resources were specified during compilation, or if the resource is not visible to the caller.</returns>
        public static Bitmap GetBitmap(String resourceName, Assembly assembly)
        {
            EmbeddedResources embeddedResources = new EmbeddedResources(assembly);

            return embeddedResources.GetBitmap(resourceName);
        }

        /// <summary>
        /// <para>Loads the specified embedded <see cref="System.Drawing.Bitmap"/> from the assembly as a <see cref="System.Windows.Forms.ImageList"/>.</para>
        /// <para>Loaded <c>ImageList</c> can be used e.g. as a <see cref="System.Windows.Forms.ListView.SmallImageList"/> of a <see cref="System.Windows.Forms.ListView"/>.</para>
        /// <para>See <c>Remarks</c> and <c>Exceptions</c> in <see cref="GetResource"/> for more information.</para>
        /// </summary>
        /// <param name="resourceName">The case-sensitive name of the embedded resource being requested.</param>
        /// <returns>A <c>ImageList</c> representing the embedded resource; <c>null</c> if no resources were specified during compilation, or if the resource is not visible to the caller.</returns>
        public ImageList GetImageList(String resourceName)
        {
            Bitmap bitmap = GetBitmap(resourceName);

            bitmap.MakeTransparent();

            int height = bitmap.Height;

            ImageList imageList = new ImageList();
            imageList.ImageSize = new Size(height, height);
            imageList.Images.AddStrip(bitmap);

            return imageList;
        }

        /// <summary>
        /// <para>Loads the specified embedded <see cref="System.Drawing.Bitmap"/> from the assembly as a <see cref="System.Windows.Forms.ImageList"/>.</para>
        /// <para>Loaded <c>ImageList</c> can be used e.g. as a <see cref="System.Windows.Forms.ListView.SmallImageList"/> of a <see cref="System.Windows.Forms.ListView"/>.</para>
        /// <para>See <c>Remarks</c> and <c>Exceptions</c> in <see cref="GetResource"/> for more information.</para>
        /// </summary>
        /// <param name="resourceName">The case-sensitive name of the embedded resource being requested.</param>
        /// <param name="assembly">Assembly to load embedded resources from. If <c>null</c>, then currently executing assembly is used.</param>
        /// <returns>A <c>ImageList</c> representing the embedded resource; <c>null</c> if no resources were specified during compilation, or if the resource is not visible to the caller.</returns>
        public static ImageList GetImageList(String resourceName, Assembly assembly)
        {
            EmbeddedResources embeddedResources = new EmbeddedResources(assembly);

            return embeddedResources.GetImageList(resourceName);
        }

        /// <summary>
        /// <para>Loads the specified embedded <see cref="System.Drawing.Icon"/> of the specified size from the assembly.</para>
        /// <para>See <c>Remarks</c> and <c>Exceptions</c> in <see cref="GetResource"/> for more information.</para>
        /// </summary>
        /// <param name="resourceName">The case-sensitive name of the embedded resource being requested.</param>
        /// <param name="size">The desired size of the icon.</param>
        /// <returns>A <c>Icon</c> representing the embedded resource; <c>null</c> if no resources were specified during compilation, or if the resource is not visible to the caller.</returns>
        /// <example>
        /// <code>The following code example demonstrates using the NotifyIcon class to display an icon for an application in the notification area.
        /// NotifyIcon notifyIcon = new NotifyIcon();
        /// notifyIcon.icon = EmbeddedResources.GetIcon("Resources.application.ico", SystemInformation.SmallIconSize);
        /// notifyIcon.Text = Application.ProductName;
        /// notifyIcon.Visible = true;
        /// </code>
        /// </example>
        public Icon GetIcon(String resourceName, Size size)
        {
            Icon icon = new Icon(GetResourceStream(resourceName), size);

            return icon;
        }

        /// <summary>
        /// <para>Loads the specified embedded <see cref="System.Drawing.Icon"/> of the specified size from the assembly.</para>
        /// <para>See <c>Remarks</c> and <c>Exceptions</c> in <see cref="GetResource"/> for more information.</para>
        /// </summary>
        /// <param name="resourceName">The case-sensitive name of the embedded resource being requested.</param>
        /// <param name="size">The desired size of the icon.</param>
        /// <param name="assembly">Assembly to load embedded resources from. If <c>null</c>, then currently executing assembly is used.</param>
        /// <returns>A <c>Icon</c> representing the embedded resource; <c>null</c> if no resources were specified during compilation, or if the resource is not visible to the caller.</returns>
        /// <example>
        /// <code>The following code example demonstrates using the NotifyIcon class to display an icon for an application in the notification area.
        /// NotifyIcon notifyIcon = new NotifyIcon();
        /// notifyIcon.icon = EmbeddedResources.GetIcon("Resources.application.ico", SystemInformation.SmallIconSize);
        /// notifyIcon.Text = Application.ProductName;
        /// notifyIcon.Visible = true;
        /// </code>
        /// </example>
        public static Icon GetIcon(String resourceName, Size size, Assembly assembly)
        {
            EmbeddedResources embeddedResources = new EmbeddedResources(assembly);

            return embeddedResources.GetIcon(resourceName, size);
        }
    }
}
