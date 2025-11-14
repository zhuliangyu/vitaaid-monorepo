using System;
using System.Collections.Generic;
using System.Runtime.CompilerServices;

namespace MySystem.Base.Extensions
{
  public static class IMESSequenceExtension
  {
    public static void AddTo(this IMESSequence self, List<IMESSequence> TargetCol)
    {
      try
      {
        self.Sequence = TargetCol.Count + 1;
        TargetCol.Add(self);
      }
      catch (Exception) { throw; }
    }
  }
}
