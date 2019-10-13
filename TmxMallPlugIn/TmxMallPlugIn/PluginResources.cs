using System;
using System.CodeDom.Compiler;
using System.ComponentModel;
using System.Diagnostics;
using System.Drawing;
using System.Globalization;
using System.Resources;
using System.Runtime.CompilerServices;
namespace TmxMallPlugIn
{
	[GeneratedCode("System.Resources.Tools.StronglyTypedResourceBuilder", "15.0.0.0"), DebuggerNonUserCode, CompilerGenerated]
	internal class PluginResources
	{
		private static ResourceManager resourceMan;
		private static CultureInfo resourceCulture;
		[EditorBrowsable(EditorBrowsableState.Advanced)]
		internal static ResourceManager ResourceManager
		{
			get
			{
				bool flag = PluginResources.resourceMan == null;
				if (flag)
				{
					ResourceManager resourceManager = new ResourceManager("TmxMallPlugIn.PluginResources", typeof(PluginResources).Assembly);
					PluginResources.resourceMan = resourceManager;
				}
				return PluginResources.resourceMan;
			}
		}
		[EditorBrowsable(EditorBrowsableState.Advanced)]
		internal static CultureInfo Culture
		{
			get
			{
				return PluginResources.resourceCulture;
			}
			set
			{
				PluginResources.resourceCulture = value;
			}
		}
		internal static Icon Icon
		{
			get
			{
				object @object = PluginResources.ResourceManager.GetObject("Icon", PluginResources.resourceCulture);
				return (Icon)@object;
			}
		}
		internal static string Plugin_Description
		{
			get
			{
				return PluginResources.ResourceManager.GetString("Plugin_Description", PluginResources.resourceCulture);
			}
		}
		internal static string Plugin_Name
		{
			get
			{
				return PluginResources.ResourceManager.GetString("Plugin_Name", PluginResources.resourceCulture);
			}
		}
		internal static string Plugin_NiceName
		{
			get
			{
				return PluginResources.ResourceManager.GetString("Plugin_NiceName", PluginResources.resourceCulture);
			}
		}
		internal static string Plugin_Tooltip
		{
			get
			{
				return PluginResources.ResourceManager.GetString("Plugin_Tooltip", PluginResources.resourceCulture);
			}
		}
		internal PluginResources()
		{
		}
	}
}
