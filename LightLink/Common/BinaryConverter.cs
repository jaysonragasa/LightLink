using System.Text;

namespace LightLink.Common;

public static class BinaryConverter
{
	/// <summary>
	/// Converts a standard string into its binary representation.
	/// </summary>
	/// <param name="text">The input string.</param>
	/// <returns>A string of 1s and 0s.</returns>
	public static string StringToBinary(string text)
	{
		if (string.IsNullOrEmpty(text))
		{
			return string.Empty;
		}

		StringBuilder binaryStringBuilder = new StringBuilder();
		foreach (char c in text)
		{
			// Convert each character to a binary string, padded to 8 bits.
			binaryStringBuilder.Append(Convert.ToString(c, 2).PadLeft(8, '0'));
		}
		return binaryStringBuilder.ToString();
	}

	/// <summary>
	/// Converts a binary string back to a standard string.
	/// </summary>
	/// <param name="binary">The input string of 1s and 0s.</param>
	/// <returns>The decoded string.</returns>
	public static string BinaryToString(string binary)
	{
		if (string.IsNullOrEmpty(binary) || binary.Length % 8 != 0)
		{
			// Or throw an exception for invalid format
			return "Invalid Binary";
		}

		StringBuilder textStringBuilder = new StringBuilder();
		for (int i = 0; i < binary.Length; i += 8)
		{
			string eightBits = binary.Substring(i, 8);
			textStringBuilder.Append((char)Convert.ToByte(eightBits, 2));
		}
		return textStringBuilder.ToString();
	}
}
