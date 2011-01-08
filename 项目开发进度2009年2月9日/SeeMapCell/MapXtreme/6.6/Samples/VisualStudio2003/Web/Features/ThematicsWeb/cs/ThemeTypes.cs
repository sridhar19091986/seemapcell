using System;
using System.Collections;

namespace ThematicsWeb
{
	/// <summary>
	/// Summary description for ThemeAndModifierTypes.
	/// </summary>
	public enum ThemeAndModifierTypes
	{
		RangedTheme,
		IndividualValueTheme,
		FeatureOverrideStyleModifier,
		DotDensityTheme,
		BarTheme,
		PieTheme,
		GraduatedSymbolTheme,
		Label_IndividualValueLabelTheme,
		Label_OverrideLabelModifier,
		Label_RangedLabelTheme
	}

	public class ThemesAndModifiers
	{
		private static Hashtable _hash = null;

		static ThemesAndModifiers()
		{
			_hash = new Hashtable();
			_hash.Add(ThemeAndModifierTypes.BarTheme.ToString(), ThemeAndModifierTypes.BarTheme);
			_hash.Add(ThemeAndModifierTypes.PieTheme.ToString(), ThemeAndModifierTypes.PieTheme);
			_hash.Add(ThemeAndModifierTypes.RangedTheme.ToString(), ThemeAndModifierTypes.RangedTheme);
			_hash.Add(ThemeAndModifierTypes.IndividualValueTheme.ToString(), ThemeAndModifierTypes.IndividualValueTheme);
			_hash.Add(ThemeAndModifierTypes.GraduatedSymbolTheme.ToString(), ThemeAndModifierTypes.GraduatedSymbolTheme);
			_hash.Add(ThemeAndModifierTypes.FeatureOverrideStyleModifier.ToString(), ThemeAndModifierTypes.FeatureOverrideStyleModifier);
			_hash.Add(ThemeAndModifierTypes.DotDensityTheme.ToString(), ThemeAndModifierTypes.DotDensityTheme);
			_hash.Add(ThemeAndModifierTypes.Label_IndividualValueLabelTheme.ToString(), ThemeAndModifierTypes.Label_IndividualValueLabelTheme);
			_hash.Add(ThemeAndModifierTypes.Label_RangedLabelTheme.ToString(), ThemeAndModifierTypes.Label_RangedLabelTheme);
			_hash.Add(ThemeAndModifierTypes.Label_OverrideLabelModifier.ToString(), ThemeAndModifierTypes.Label_OverrideLabelModifier);
		}

		public static ThemeAndModifierTypes GetThemeAndModifierTypesByName(string themeName)
		{
			if(_hash.Contains(themeName))
			{
				return (ThemeAndModifierTypes)_hash[themeName];
			}
			else
			{
				throw new ArgumentException("Wrong themeName!!!");
			}
		}

		public static void FillThemeNames(System.Collections.IList list)
		{
			IEnumerator keys = _hash.Keys.GetEnumerator();
			while(keys.MoveNext())
			{
				list.Add(_hash[keys.Current].ToString());
			}
		}
	}

}
