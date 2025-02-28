// Decompiled with JetBrains decompiler
// Type: DNACopier
// Assembly: Assembly-CSharp, Version=0.0.0.0, Culture=neutral, PublicKeyToken=null
// MVID: 7AE521BE-DDCA-4BB1-9869-8132CF2A08FD
// Assembly location: C:\Users\85206\Desktop\Oxide.ReignOfKings-develop\src\Dependencies\windows\ROK_Data\Managed\Assembly-CSharp.dll

using UMA;

#nullable disable
public static class DNACopier
{
  public static UMADnaHumanoid CopyDNAOver(UMADnaHumanoid copyTo, UMADnaHumanoid copyFrom)
  {
    copyTo.armLength = copyFrom.armLength;
    copyTo.armWidth = copyFrom.armWidth;
    copyTo.belly = copyFrom.belly;
    copyTo.breastSize = copyFrom.breastSize;
    copyTo.cheekPosition = copyFrom.cheekPosition;
    copyTo.cheekSize = copyFrom.cheekSize;
    copyTo.chinPosition = copyFrom.chinPosition;
    copyTo.chinPronounced = copyFrom.chinPronounced;
    copyTo.chinSize = copyFrom.chinSize;
    copyTo.earsPosition = copyFrom.earsPosition;
    copyTo.earsRotation = copyFrom.earsRotation;
    copyTo.earsSize = copyFrom.earsSize;
    copyTo.eyeRotation = copyFrom.eyeRotation;
    copyTo.eyeSize = copyFrom.eyeSize;
    copyTo.feetSize = copyFrom.feetSize;
    copyTo.forearmLength = copyFrom.forearmLength;
    copyTo.forearmWidth = copyFrom.forearmWidth;
    copyTo.foreheadPosition = copyFrom.foreheadPosition;
    copyTo.foreheadSize = copyFrom.foreheadSize;
    copyTo.gluteusSize = copyFrom.gluteusSize;
    copyTo.handsSize = copyFrom.handsSize;
    copyTo.headSize = copyFrom.headSize;
    copyTo.headWidth = copyFrom.headWidth;
    copyTo.height = copyFrom.height;
    copyTo.jawsPosition = copyFrom.jawsPosition;
    copyTo.jawsSize = copyFrom.jawsSize;
    copyTo.legSeparation = copyFrom.legSeparation;
    copyTo.legsSize = copyFrom.legsSize;
    copyTo.lipsSize = copyFrom.lipsSize;
    copyTo.lowCheekPosition = copyFrom.lowCheekPosition;
    copyTo.lowCheekPronounced = copyFrom.lowCheekPronounced;
    copyTo.lowerMuscle = copyFrom.lowerMuscle;
    copyTo.lowerWeight = copyFrom.lowerWeight;
    copyTo.mandibleSize = copyFrom.mandibleSize;
    copyTo.mouthSize = copyFrom.mouthSize;
    copyTo.neckThickness = copyFrom.neckThickness;
    copyTo.noseCurve = copyFrom.noseCurve;
    copyTo.noseFlatten = copyFrom.noseFlatten;
    copyTo.noseInclination = copyFrom.noseInclination;
    copyTo.nosePosition = copyFrom.nosePosition;
    copyTo.nosePronounced = copyFrom.nosePronounced;
    copyTo.noseSize = copyFrom.noseSize;
    copyTo.noseWidth = copyFrom.noseWidth;
    copyTo.upperMuscle = copyFrom.upperMuscle;
    copyTo.upperWeight = copyFrom.upperWeight;
    copyTo.waist = copyFrom.waist;
    return copyTo;
  }
}
