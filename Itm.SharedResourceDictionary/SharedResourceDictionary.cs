﻿using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Windows;

namespace Itm.SharedResourceDictionary
{
	/// <summary>
	/// The shared resource dictionary is a specialized resource dictionary
	/// that loads it content only once. If a second instance with the same source
	/// is created, it only merges the resources from the cache.
	/// </summary>
	public class SharedResourceDictionary : ResourceDictionary
	{
		/// <summary>
		/// Internal cache of loaded dictionaries 
		/// </summary>
		public static Dictionary<Uri, ResourceDictionary> _sharedDictionaries =
			new Dictionary<Uri, ResourceDictionary>();

		/// <summary>
		/// Local member of the source uri
		/// </summary>
		private Uri _sourceUri;

		private static bool IsInDesignMode
		{
			get
			{
				return (bool)DependencyPropertyDescriptor.FromProperty(DesignerProperties.IsInDesignModeProperty,
						 typeof(DependencyObject)).Metadata.DefaultValue;
			}
		}

		public string SourcePath { get; set; }

		/// <summary>
		/// Gets or sets the uniform resource identifier (URI) to load resources from.
		/// </summary>
		public new Uri Source
		{
			get {
				if (IsInDesignMode)
				{
					return base.Source;
				}
				else
				{
					return _sourceUri;
				}
			}
			set
			{
				if (value == null)
					return;

				if (IsInDesignMode == true)
				{
					var dict = Application.LoadComponent(new Uri(SourcePath, UriKind.Relative)) as ResourceDictionary;
					MergedDictionaries.Add(dict);
					return;
				}

				_sourceUri = value;

				if (_sharedDictionaries.ContainsKey(value) == false)
				{
					// If the dictionary is not yet loaded, load it by setting
					// the source of the base class
					base.Source = value;

					// add it to the cache
					_sharedDictionaries.Add(value, this);
				}
				else
				{
					// If the dictionary is already loaded, get it from the cache
					MergedDictionaries.Add(_sharedDictionaries[value]);
				}
			}
		}
	}
}
