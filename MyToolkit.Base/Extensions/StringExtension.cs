using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace MyToolkit.Base.Extensions
{
	public static class StringExtension
	{
		public static String[] SplitEx(this String source, params char[] separator)
		{
			return source.SplitEx(separator, '"');
		}
		public static String[] SplitEx(this String source, char[] separator, char qualifier, int count = Int32.MaxValue, StringSplitOptions options = StringSplitOptions.None)
		{
			int[] sepList = new int[source.Length];
			int numReplaces = source.MakeSeparatorListEx(separator, ref sepList, qualifier);
			bool omitEmptyEntries = (options == StringSplitOptions.RemoveEmptyEntries);
			String[] stringArray;
			//Handle the special case of no replaces and special count.
			if (0 == numReplaces || count == 1)
			{
				stringArray = new String[1];
				stringArray[0] = source;
				return stringArray;
			}
			if (omitEmptyEntries)
			{
				stringArray = source.InternalSplitOmitEmptyEntriesEx(sepList, null, numReplaces, count, qualifier);
			}
			else
			{
				stringArray = source.InternalSplitKeepEmptyEntriesEx(sepList, null, numReplaces, count, qualifier);
			}

			return stringArray;
		}


		// This function will not keep the Empty String 
		private static String[] InternalSplitOmitEmptyEntriesEx(this String source, Int32[] sepList, Int32[] lengthList, Int32 numReplaces, int count, char qualifier)
		{
			// Allocate array to hold items. This array may not be 
			// filled completely in this function, we will create a 
			// new array and copy string references to that new array.

			int maxItems = (numReplaces < count) ? (numReplaces + 1) : count;
			String[] splitStrings = new String[maxItems];

			int currIndex = 0;
			int arrIndex = 0;

			for (int i = 0; i < numReplaces && currIndex < source.Length; i++)
			{
				if (sepList[i] - currIndex > 0)
				{
					splitStrings[arrIndex++] = source.Substring(currIndex, sepList[i] - currIndex);
				}
				currIndex = sepList[i] + ((lengthList == null) ? 1 : lengthList[i]);
				if (arrIndex == count - 1)
				{
					// If all the remaining entries at the end are empty, skip them
					while (i < numReplaces - 1 && currIndex == sepList[++i])
					{
						currIndex += ((lengthList == null) ? 1 : lengthList[i]);
					}
					break;
				}
			}


			//Handle the last string at the end of the array if there is one.
			if (currIndex < source.Length)
			{
				splitStrings[arrIndex++] = source.Substring(currIndex);
			}

			String[] stringArray = splitStrings;
			if (arrIndex != maxItems)
			{
				stringArray = new String[arrIndex];
				for (int j = 0; j < arrIndex; j++)
				{
					var str = splitStrings[j];
					if (qualifier != ' ' && str.Length > 2 &&
						str[0] == qualifier && str[str.Length - 1] == qualifier)
						stringArray[j] = str.Substring(1, str.Length - 2);
					else
						stringArray[j] = str;
				}
			}
			return stringArray;
		}

		// Note a few special case in this function:
		//     If there is no separator in the string, a string array which only contains 
		//     the original string will be returned regardless of the count. 
		//

		private static String[] InternalSplitKeepEmptyEntriesEx(this String source, Int32[] sepList, Int32[] lengthList, Int32 numReplaces, int count, char qualifier)
		{
			int currIndex = 0;
			int arrIndex = 0;

			count--;
			int numActualReplaces = (numReplaces < count) ? numReplaces : count;

			//Allocate space for the new array.
			//+1 for the string from the end of the last replace to the end of the String.
			String[] splitStrings = new String[numActualReplaces + 1];

			for (int i = 0; i < numActualReplaces && currIndex < source.Length; i++)
			{
				var str = source.Substring(currIndex, sepList[i] - currIndex);
				if (qualifier != ' ' && str.Length > 2 &&
					str[0] == qualifier && str[str.Length - 1] == qualifier)
					splitStrings[arrIndex++] = str.Substring(1, str.Length - 2);
				else
					splitStrings[arrIndex++] = str;

				currIndex = sepList[i] + ((lengthList == null) ? 1 : lengthList[i]);
			}

			//Handle the last string at the end of the array if there is one.
			if (currIndex < source.Length && numActualReplaces >= 0)
			{
				var str = source.Substring(currIndex);
				if (qualifier != ' ' && str.Length > 2 &&
					str[0] == qualifier && str[str.Length - 1] == qualifier)
					splitStrings[arrIndex] = str.Substring(1, str.Length - 2);
				else
					splitStrings[arrIndex] = str;
			}
			else if (arrIndex == numActualReplaces)
			{
				//We had a separator character at the end of a string.  Rather than just allowing
				//a null character, we'll replace the last element in the array with an empty string.
				splitStrings[arrIndex] = String.Empty;

			}

			return splitStrings;
		}
		//--------------------------------------------------------------------    
		// This function returns number of the places within baseString where 
		// instances of characters in Separator occur.         
		// Args: separator  -- A string containing all of the split characters.
		//       sepList    -- an array of ints for split char indicies.
		//--------------------------------------------------------------------    
		[System.Security.SecuritySafeCritical]  // auto-generated
		private static unsafe int MakeSeparatorListEx(this String source, char[] separator, ref int[] sepList, char qualifier)
		{
			int foundCount = 0;
			var charArray = source.ToArray();
			bool bInQualifier = false;
			if (separator == null || separator.Length == 0)
			{
				fixed (char* pwzChars = &(charArray[0]))
				{
					//If they passed null or an empty string, look for whitespace.
					for (int i = 0; i < source.Length && foundCount < sepList.Length; i++)
					{
						if (pwzChars[i] == qualifier)
						{
							if (i == 0 || (foundCount > 0 && sepList[foundCount - 1] == i - 1)) // begin of token
								bInQualifier = true;
							else if (bInQualifier == true)
								bInQualifier = false;
						}
						else if (Char.IsWhiteSpace(pwzChars[i]) && bInQualifier == false)
						{
							sepList[foundCount++] = i;
						}

					}
				}
			}
			else
			{
				int sepListCount = sepList.Length;
				int sepCount = separator.Length;
				//If they passed in a string of chars, actually look for those chars.
				fixed (char* pwzChars = &(charArray[0]), pSepChars = separator)
				{
					for (int i = 0; i < source.Length && foundCount < sepListCount; i++)
					{
						if (pwzChars[i] == qualifier)
						{
							if (i == 0 || (foundCount > 0 && sepList[foundCount - 1] == i - 1)) // begin of token
								bInQualifier = true;
							else if (bInQualifier == true)
								bInQualifier = false;
						}
						else if (bInQualifier == false)
						{
							char* pSep = pSepChars;
							for (int j = 0; j < sepCount; j++, pSep++)
							{
								if (pwzChars[i] == *pSep)
								{
									sepList[foundCount++] = i;
									break;
								}
							}
						}
					}
				}
			}
			return foundCount;
		}

		//--------------------------------------------------------------------    
		// This function returns number of the places within baseString where 
		// instances of separator strings occur.         
		// Args: separators -- An array containing all of the split strings.
		//       sepList    -- an array of ints for split string indicies.
		//       lengthList -- an array of ints for split string lengths.
		//--------------------------------------------------------------------    
		[System.Security.SecuritySafeCritical]  // auto-generated
		private static unsafe int MakeSeparatorListEx(this String source, String[] separators, ref int[] sepList, ref int[] lengthList, char qualifier)
		{
			if (separators == null || separators.Length == 0) return 0;
			//, "separators != null && separators.Length > 0");

			int foundCount = 0;
			int sepListCount = sepList.Length;
			int sepCount = separators.Length;
			var charArray = source.ToArray();
			bool bInQualifier = false;

			fixed (char* pwzChars = &(charArray[0]))
			{
				for (int i = 0; i < source.Length && foundCount < sepListCount; i++)
				{
					if (pwzChars[i] == qualifier)
					{
						if (i == 0 || (foundCount > 0 && sepList[foundCount - 1] == i - 1)) // begin of token
							bInQualifier = true;
						else if (bInQualifier == true)
							bInQualifier = false;
					}
					else if (bInQualifier == false)
					{
						for (int j = 0; j < separators.Length; j++)
						{
							String separator = separators[j];
							if (String.IsNullOrEmpty(separator))
							{
								continue;
							}
							Int32 currentSepLength = separator.Length;
							if (pwzChars[i] == separator[0] && currentSepLength <= source.Length - i)
							{
								if (currentSepLength == 1
									|| String.CompareOrdinal(source, i, separator, 0, currentSepLength) == 0)
								{
									sepList[foundCount] = i;
									lengthList[foundCount] = currentSepLength;
									foundCount++;
									i += currentSepLength - 1;
									break;
								}
							}
						}
					}
				}
			}
			return foundCount;
		}

	}
}
