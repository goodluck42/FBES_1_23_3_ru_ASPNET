using System;
using Microsoft.CodeAnalysis;

namespace ShadowsPlayground.SourceGenerators.AutoException;

public static class AccessModifierHelper
{
	public static string GetAccessModifierString(Accessibility accessibility)
	{
		return accessibility switch
		{
			Accessibility.Private => "private",
			Accessibility.ProtectedAndInternal => "private protected",
			Accessibility.Protected => "protected",
			Accessibility.Internal => "internal",
			Accessibility.ProtectedOrInternal => "protected internal",
			Accessibility.Public => "public",
			_ => throw new ArgumentOutOfRangeException()
		};
	}
}