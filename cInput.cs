// Decompiled with JetBrains decompiler
// Type: cInput
// Assembly: Assembly-CSharp, Version=0.0.0.0, Culture=neutral, PublicKeyToken=null
// MVID: 7AE521BE-DDCA-4BB1-9869-8132CF2A08FD
// Assembly location: C:\Users\85206\Desktop\Oxide.ReignOfKings-develop\src\Dependencies\windows\ROK_Data\Managed\Assembly-CSharp.dll

using System.Collections.Generic;
using System.Text;
using UnityEngine;

#nullable disable
public class cInput : MonoBehaviour
{
  private const int MAX_INPUTS = 64;
  public static float gravity = 1f;
  public static float sensitivity = 1f;
  public static float deadzone = 0.1f;
  private static bool _allowDuplicates = false;
  private static string[,] _defaultStrings = new string[64, 5];
  private static Dictionary<int, int> _inputNameHash = new Dictionary<int, int>();
  private static string[] _inputName = new string[64];
  private static KeyCode[] _inputPrimary = new KeyCode[64];
  private static KeyCode[] _modifierUsedPrimary = new KeyCode[64];
  private static KeyCode[] _inputSecondary = new KeyCode[64];
  private static KeyCode[] _modifierUsedSecondary = new KeyCode[64];
  private static List<KeyCode> _modifiers = new List<KeyCode>();
  private static List<int> _markedAsAxis = new List<int>();
  private static Dictionary<int, int> _axisNameHash = new Dictionary<int, int>();
  private static string[] _axisName = new string[64];
  private static string[] _axisPrimary = new string[64];
  private static string[] _axisSecondary = new string[64];
  private static float[,] _defaultAxes = new float[64, 3];
  private static float[] _individualAxisSens = new float[64];
  private static float[] _individualAxisGrav = new float[64];
  private static float[] _individualAxisDead = new float[64];
  private static bool[] _invertAxis = new bool[64];
  private static int[,] _makeAxis = new int[64, 2];
  private static int _inputLength = -1;
  private static int _axisLength = -1;
  private static List<KeyCode> _forbiddenKeys = new List<KeyCode>();
  private static List<KeyCode>[] _inputLeaksPrimary = new List<KeyCode>[64];
  private static List<KeyCode>[] _inputLeaksSecondary = new List<KeyCode>[64];
  private static bool[] _getKeyArray = new bool[64];
  private static bool[] _getKeyDownArray = new bool[64];
  private static bool[] _getKeyUpArray = new bool[64];
  private static bool[] _axisTriggerArrayPrimary = new bool[64];
  private static bool[] _axisTriggerArraySecondary = new bool[64];
  private static float[] _getAxis = new float[64];
  private static float[] _getAxisRaw = new float[64];
  private static float[] _getAxisArray = new float[64];
  private static float[] _getAxisArrayRaw = new float[64];
  private static bool _allowMouseAxis = false;
  private static bool _allowMouseButtons = true;
  private static bool _allowJoystickButtons = true;
  private static bool _allowJoystickAxis = true;
  private static bool _allowKeyboard = true;
  private static int _numGamepads = 4;
  private static string _menuHeaderString = "label";
  private static string _menuActionsString = "box";
  private static string _menuInputsString = "box";
  private static string _menuButtonsString = "button";
  private static bool _scanning;
  private static int _cScanIndex;
  private static int _cScanInput;
  private static cInput _cObject;
  private static bool _hascObject;
  private static bool _cKeysLoaded;
  private static Dictionary<string, float> _axisRawValues = new Dictionary<string, float>();
  private static string _exAllowDuplicates;
  private static string _exAxis;
  private static string _exAxisInverted;
  private static string _exDefaults;
  private static string _exInputs;
  private static string _exCalibrations;
  private static string _exCalibrationValues;
  private static bool _externalSaving = false;
  private static Dictionary<string, KeyCode> _string2Key = new Dictionary<string, KeyCode>();
  private static int[] _axisType = new int[10 * cInput._numGamepads];
  private static Dictionary<string, float> _axisCalibrationOffset = new Dictionary<string, float>();
  private static string[,] _joyStrings = new string[cInput._numGamepads + 1, 11];
  private static string[,] _joyStringsPos = new string[cInput._numGamepads + 1, 11];
  private static string[,] _joyStringsNeg = new string[cInput._numGamepads + 1, 11];
  private static Dictionary<string, int[]> _joyStringsIndices = new Dictionary<string, int[]>();
  private static Dictionary<string, int[]> _joyStringsPosIndices = new Dictionary<string, int[]>();
  private static Dictionary<string, int[]> _joyStringsNegIndices = new Dictionary<string, int[]>();

  public static bool anyKey { get; private set; }

  public static bool anyKeyUp { get; private set; }

  public static bool anyKeyDown { get; private set; }

  public static bool scanning
  {
    get => cInput._scanning;
    set => cInput._scanning = value;
  }

  public static int length
  {
    get
    {
      cInput._cInputInit();
      return cInput._inputLength + 1;
    }
  }

  public static bool allowDuplicates
  {
    get
    {
      cInput._cInputInit();
      return cInput._allowDuplicates;
    }
    set
    {
      cInput._allowDuplicates = value;
      PlayerPrefs.SetString("cInput_dubl", value.ToString());
      cInput._exAllowDuplicates = value.ToString();
    }
  }

  public static bool AllowMouseAxis
  {
    get => cInput._allowMouseAxis;
    set => cInput._allowMouseAxis = value;
  }

  public static bool AllowMouseButtons
  {
    get => cInput._allowMouseButtons;
    set => cInput._allowMouseButtons = value;
  }

  public static bool AllowJoystickButtons
  {
    get => cInput._allowJoystickButtons;
    set => cInput._allowJoystickButtons = value;
  }

  public static bool AllowJoystickAxis
  {
    get => cInput._allowJoystickAxis;
    set => cInput._allowJoystickAxis = value;
  }

  public static bool AllowKeyboard
  {
    get => cInput._allowKeyboard;
    set => cInput._allowKeyboard = value;
  }

  private void Awake()
  {
    Object.DontDestroyOnLoad((Object) this);
    cInput._cObject = this;
    cInput._hascObject = true;
    for (int index = 0; index < 64; ++index)
    {
      cInput._individualAxisSens[index] = -99f;
      cInput._individualAxisGrav[index] = -99f;
      cInput._individualAxisDead[index] = -99f;
    }
    for (int index = 0; index < cInput._inputLeaksPrimary.Length; ++index)
    {
      cInput._inputLeaksPrimary[index] = new List<KeyCode>(2);
      cInput._inputLeaksSecondary[index] = new List<KeyCode>(2);
    }
  }

  private void Start()
  {
    cInput._CreateDictionary();
    if (cInput._externalSaving)
      cInput._LoadExternalInputs();
    else
      cInput._LoadInputs();
    cInput.AddModifier(KeyCode.None);
  }

  private void Update()
  {
    if (cInput._scanning)
    {
      if (cInput._cScanInput != 0)
      {
        this._InputScans();
      }
      else
      {
        string primary = !string.IsNullOrEmpty(cInput._axisPrimary[cInput._cScanIndex]) ? cInput._axisPrimary[cInput._cScanIndex] : cInput._inputPrimary[cInput._cScanIndex].ToString();
        string secondary = !string.IsNullOrEmpty(cInput._axisSecondary[cInput._cScanIndex]) ? cInput._axisSecondary[cInput._cScanIndex] : cInput._inputSecondary[cInput._cScanIndex].ToString();
        cInput._ChangeKey(cInput._cScanIndex, cInput._inputName[cInput._cScanIndex], primary, secondary);
        cInput._scanning = false;
      }
    }
    else
      this._CheckInputs();
  }

  public static void Init() => cInput._cInputInit();

  private static void _CreateDictionary()
  {
    if (cInput._string2Key.Count != 0)
      return;
    for (int index = 0; index <= 429; ++index)
    {
      KeyCode keyCode = (KeyCode) index;
      cInput._string2Key.Add(keyCode.ToString(), keyCode);
    }
    for (int index1 = 1; index1 <= cInput._numGamepads; ++index1)
    {
      for (int index2 = 1; index2 <= 10; ++index2)
      {
        StringBuilder stringBuilder = new StringBuilder("Joy" + (object) index1 + " Axis " + (object) index2);
        cInput._joyStrings[index1, index2] = stringBuilder.ToString();
        cInput._joyStringsIndices.Add(stringBuilder.ToString(), new int[2]
        {
          index1,
          index2
        });
        stringBuilder.Append("+");
        cInput._joyStringsPos[index1, index2] = stringBuilder.ToString();
        cInput._joyStringsPosIndices.Add(stringBuilder.ToString(), new int[2]
        {
          index1,
          index2
        });
        stringBuilder.Replace('+', '-');
        cInput._joyStringsNeg[index1, index2] = stringBuilder.ToString();
        cInput._joyStringsNegIndices.Add(stringBuilder.ToString(), new int[2]
        {
          index1,
          index2
        });
      }
    }
  }

  public static KeyCode ConvertToKey(string str) => cInput._ConvertString2Key(str);

  private static KeyCode _ConvertString2Key(string str)
  {
    if (string.IsNullOrEmpty(str))
      return KeyCode.None;
    if (cInput._string2Key.Count == 0)
      cInput._CreateDictionary();
    if (cInput._string2Key.ContainsKey(str))
      return cInput._string2Key[str];
    if (!cInput._IsAxisValid(str))
      Logger.InfoFormat("cInput error: {0} is not a valid input.", (object) str);
    return KeyCode.None;
  }

  public static void ForbidKey(KeyCode key)
  {
    cInput._cInputInit();
    if (cInput._forbiddenKeys.Contains(key))
      return;
    cInput._forbiddenKeys.Add(key);
  }

  public static void ForbidKey(string keyString)
  {
    cInput._cInputInit();
    cInput.ForbidKey(cInput._ConvertString2Key(keyString));
  }

  public static KeyCode[] GetForbidKeys()
  {
    cInput._cInputInit();
    return cInput._forbiddenKeys.ToArray();
  }

  public static void DefaultModifier(KeyCode modifierKey)
  {
    cInput._cInputInit();
    if (cInput._modifiers.Contains(modifierKey))
      return;
    cInput._modifiers.Add(modifierKey);
  }

  public static void AddModifier(KeyCode modifierKey)
  {
    cInput._cInputInit();
    if (cInput._modifiers.Contains(modifierKey))
      return;
    cInput._modifiers.Add(modifierKey);
    cInput._SaveModifier();
  }

  public static void AddModifier(string modifier)
  {
    cInput._cInputInit();
    cInput.AddModifier(cInput._ConvertString2Key(modifier));
  }

  public static void RemoveModifier(KeyCode modifierKey)
  {
    cInput._cInputInit();
    if (!cInput._modifiers.Contains(modifierKey))
      return;
    cInput._modifiers.Remove(modifierKey);
    cInput._SaveModifier();
  }

  public static void RemoveModifier(string modifier)
  {
    cInput._cInputInit();
    cInput.RemoveModifier(cInput._ConvertString2Key(modifier));
  }

  public static KeyCode[] GetModifiers()
  {
    cInput._cInputInit();
    return cInput._modifiers.ToArray();
  }

  public static int SetKey(string action, KeyCode primary)
  {
    return cInput.SetKey(action, primary, KeyCode.None, primary, KeyCode.None);
  }

  public static int SetKey(string action, KeyCode primary, KeyCode secondary)
  {
    return cInput.SetKey(action, primary, secondary, primary, secondary);
  }

  public static int SetKey(
    string action,
    KeyCode primary,
    KeyCode secondary,
    KeyCode primaryModifier)
  {
    return cInput.SetKey(action, primary, secondary, primaryModifier, secondary);
  }

  public static int SetKey(
    string action,
    KeyCode primary,
    KeyCode secondary,
    KeyCode primaryModifier,
    KeyCode secondaryModifier)
  {
    cInput._cInputInit();
    if (primaryModifier == KeyCode.None)
      primaryModifier = primary;
    if (secondaryModifier == KeyCode.None)
      secondaryModifier = secondary;
    if (cInput._FindKeyByDescription(action) == -1)
      cInput._SetDefaultKey(cInput._inputLength + 1, action, primary, secondary, primaryModifier, secondaryModifier);
    return action.GetHashCode();
  }

  public static int SetKey(
    string action,
    string primary,
    string secondary,
    KeyCode primaryModifier)
  {
    return cInput.SetKey(action, primary, secondary, primaryModifier, KeyCode.None);
  }

  public static int SetKey(
    string action,
    string primary,
    string secondary,
    KeyCode primaryModifier,
    KeyCode secondaryModifier)
  {
    cInput._cInputInit();
    string pMod = primaryModifier.ToString();
    string sMod = secondaryModifier.ToString();
    if (string.IsNullOrEmpty(pMod) || pMod == "None")
      pMod = primary;
    if (string.IsNullOrEmpty(sMod) || sMod == "None")
      sMod = secondary;
    if (cInput._FindKeyByDescription(action) == -1)
      cInput._SetDefaultKey(cInput._inputLength + 1, action, primary, secondary, pMod, sMod);
    return action.GetHashCode();
  }

  public static int SetKey(
    string action,
    string primary,
    KeyCode secondary,
    KeyCode primaryModifier,
    KeyCode secondaryModifier)
  {
    cInput._cInputInit();
    string pMod = primaryModifier.ToString();
    if (string.IsNullOrEmpty(pMod) || pMod == "None")
      pMod = primary;
    if (secondaryModifier == KeyCode.None)
      secondaryModifier = secondary;
    if (cInput._FindKeyByDescription(action) == -1)
      cInput._SetDefaultKey(cInput._inputLength + 1, action, primary, secondary.ToString(), pMod, secondaryModifier.ToString());
    return action.GetHashCode();
  }

  public static int SetKey(
    string action,
    KeyCode primary,
    string secondary,
    KeyCode primaryModifier,
    KeyCode secondaryModifier)
  {
    cInput._cInputInit();
    string sMod = secondaryModifier.ToString();
    if (primaryModifier == KeyCode.None)
      primaryModifier = primary;
    if (string.IsNullOrEmpty(sMod) || sMod == "None")
      sMod = secondary;
    if (cInput._FindKeyByDescription(action) == -1)
      cInput._SetDefaultKey(cInput._inputLength + 1, action, primary.ToString(), secondary, primaryModifier.ToString(), sMod);
    return action.GetHashCode();
  }

  public static int SetKey(string action, string primary)
  {
    return cInput.SetKey(action, primary, "None", primary, "None");
  }

  public static int SetKey(string action, string primary, string secondary)
  {
    return cInput.SetKey(action, primary, secondary, primary, secondary);
  }

  public static int SetKey(
    string action,
    string primary,
    string secondary,
    string primaryModifier)
  {
    return cInput.SetKey(action, primary, secondary, primaryModifier, secondary);
  }

  public static int SetKey(
    string action,
    string primary,
    string secondary,
    string primaryModifier,
    string secondaryModifier)
  {
    cInput._cInputInit();
    if (string.IsNullOrEmpty(primaryModifier) || primaryModifier == "None")
      primaryModifier = primary;
    if (string.IsNullOrEmpty(secondaryModifier) || secondaryModifier == "None")
      secondaryModifier = secondary;
    if (cInput._FindKeyByDescription(action) == -1)
      cInput._SetDefaultKey(cInput._inputLength + 1, action, primary, secondary, primaryModifier, secondaryModifier);
    return action.GetHashCode();
  }

  private static void _SetDefaultKey(
    int _num,
    string _name,
    KeyCode _input1,
    KeyCode _input2,
    KeyCode pMod,
    KeyCode sMod)
  {
    cInput._defaultStrings[_num, 0] = _name;
    cInput._defaultStrings[_num, 1] = _input1.ToString();
    cInput._defaultStrings[_num, 2] = _input2.ToString();
    cInput._defaultStrings[_num, 3] = pMod.ToString();
    cInput._defaultStrings[_num, 4] = sMod.ToString();
    int hashCode = _name.GetHashCode();
    if (!cInput._inputNameHash.ContainsKey(hashCode))
      cInput._inputNameHash.Add(hashCode, _num);
    if (_num > cInput._inputLength)
      cInput._inputLength = _num;
    cInput._modifierUsedPrimary[_num] = cInput._ConvertString2Key(cInput._defaultStrings[_num, 3]);
    cInput._modifierUsedSecondary[_num] = cInput._ConvertString2Key(cInput._defaultStrings[_num, 4]);
    cInput._SetKey(_num, _name, _input1, _input2);
    cInput._SaveDefaults();
  }

  private static void _SetDefaultKey(
    int _num,
    string _name,
    string _input1,
    string _input2,
    string pMod,
    string sMod)
  {
    cInput._defaultStrings[_num, 0] = _name;
    cInput._defaultStrings[_num, 1] = _input1;
    cInput._defaultStrings[_num, 2] = !string.IsNullOrEmpty(_input2) ? _input2 : KeyCode.None.ToString();
    cInput._defaultStrings[_num, 3] = !string.IsNullOrEmpty(pMod) ? pMod : _input1;
    cInput._defaultStrings[_num, 4] = !string.IsNullOrEmpty(sMod) ? sMod : _input2;
    int hashCode = _name.GetHashCode();
    if (!cInput._inputNameHash.ContainsKey(hashCode))
      cInput._inputNameHash.Add(hashCode, _num);
    if (_num > cInput._inputLength)
      cInput._inputLength = _num;
    cInput._modifierUsedPrimary[_num] = cInput._ConvertString2Key(cInput._defaultStrings[_num, 3]);
    cInput._modifierUsedSecondary[_num] = cInput._ConvertString2Key(cInput._defaultStrings[_num, 4]);
    cInput._SetKey(_num, _name, _input1, _input2);
    cInput._SaveDefaults();
  }

  private static void _SetKey(int _num, string _name, KeyCode _input1, KeyCode _input2)
  {
    cInput._inputName[_num] = _name;
    cInput._axisPrimary[_num] = string.Empty;
    if (cInput._string2Key.Count == 0)
      return;
    if (_input1 != KeyCode.None)
    {
      cInput._inputPrimary[_num] = _input1;
      string axisName = cInput._ChangeStringToAxisName(_input1.ToString());
      if (_input1.ToString() != axisName)
        cInput._axisPrimary[_num] = _input1.ToString();
    }
    cInput._axisSecondary[_num] = string.Empty;
    if (_input2 == KeyCode.None)
      return;
    cInput._inputSecondary[_num] = _input2;
    string axisName1 = cInput._ChangeStringToAxisName(_input2.ToString());
    if (!(_input2.ToString() != axisName1))
      return;
    cInput._axisSecondary[_num] = _input2.ToString();
  }

  private static void _SetKey(int _num, string _name, string _input1, string _input2)
  {
    cInput._inputName[_num] = _name;
    cInput._axisPrimary[_num] = string.Empty;
    if (cInput._string2Key.Count == 0)
      return;
    if (!string.IsNullOrEmpty(_input1))
    {
      KeyCode keyCode = cInput._ConvertString2Key(_input1);
      cInput._inputPrimary[_num] = keyCode;
      string axisName = cInput._ChangeStringToAxisName(_input1);
      if (_input1 != axisName)
        cInput._axisPrimary[_num] = _input1;
    }
    cInput._axisSecondary[_num] = string.Empty;
    if (!string.IsNullOrEmpty(_input2))
    {
      KeyCode keyCode = cInput._ConvertString2Key(_input2);
      cInput._inputSecondary[_num] = keyCode;
      string axisName = cInput._ChangeStringToAxisName(_input2);
      if (_input2 != axisName)
        cInput._axisSecondary[_num] = _input2;
    }
    cInput.TestPrimaryLeak(_num);
    cInput.TestSecondaryLeak(_num);
  }

  private static void TestPrimaryLeak(int index)
  {
    if (cInput._inputLeaksPrimary[index].Count > 0)
      cInput._inputLeaksPrimary[index].Clear();
    if (cInput._modifierUsedPrimary[index] != KeyCode.None && cInput._modifierUsedPrimary[index] != cInput._inputPrimary[index])
      return;
    if (!string.IsNullOrEmpty(cInput._axisPrimary[index]))
    {
      for (int index1 = 0; index1 < cInput._axisPrimary.Length; ++index1)
      {
        if (cInput._axisPrimary[index1] == cInput._axisPrimary[index] && index1 != index)
        {
          KeyCode keyCode = cInput._modifierUsedPrimary[index1];
          if (keyCode != KeyCode.None && keyCode != cInput._inputPrimary[index1] && !cInput._inputLeaksPrimary[index].Contains(keyCode))
            cInput._inputLeaksPrimary[index].Add(keyCode);
        }
      }
    }
    else
    {
      if (cInput._inputPrimary[index] == KeyCode.None)
        return;
      for (int index2 = 0; index2 < cInput._inputPrimary.Length; ++index2)
      {
        if (cInput._inputPrimary[index2] == cInput._inputPrimary[index] && index2 != index)
        {
          KeyCode keyCode = cInput._modifierUsedPrimary[index2];
          if (keyCode != KeyCode.None && keyCode != cInput._inputPrimary[index2] && !cInput._inputLeaksPrimary[index].Contains(keyCode))
            cInput._inputLeaksPrimary[index].Add(keyCode);
        }
      }
    }
  }

  private static bool CanPrimaryLeak(int index)
  {
    for (int index1 = 0; index1 < cInput._inputLeaksPrimary[index].Count; ++index1)
    {
      if (Input.GetKey(cInput._inputLeaksPrimary[index][index1]))
        return false;
    }
    return true;
  }

  private static void TestSecondaryLeak(int index)
  {
    if (cInput._inputLeaksSecondary[index].Count > 0)
      cInput._inputLeaksSecondary[index].Clear();
    if (cInput._modifierUsedSecondary[index] != KeyCode.None && cInput._modifierUsedSecondary[index] != cInput._inputSecondary[index])
      return;
    if (!string.IsNullOrEmpty(cInput._axisSecondary[index]))
    {
      for (int index1 = 0; index1 < cInput._axisSecondary.Length; ++index1)
      {
        if (cInput._axisSecondary[index1] == cInput._axisSecondary[index] && index1 != index)
        {
          KeyCode keyCode = cInput._modifierUsedSecondary[index1];
          if (keyCode != KeyCode.None && keyCode != cInput._inputSecondary[index1] && !cInput._inputLeaksSecondary[index].Contains(keyCode))
            cInput._inputLeaksSecondary[index].Add(keyCode);
        }
      }
    }
    else
    {
      if (cInput._inputSecondary[index] == KeyCode.None)
        return;
      for (int index2 = 0; index2 < cInput._inputSecondary.Length; ++index2)
      {
        if (cInput._inputSecondary[index2] == cInput._inputSecondary[index] && index2 != index)
        {
          KeyCode keyCode = cInput._modifierUsedSecondary[index2];
          if (keyCode != KeyCode.None && keyCode != cInput._inputSecondary[index2] && !cInput._inputLeaksSecondary[index].Contains(keyCode))
            cInput._inputLeaksSecondary[index].Add(keyCode);
        }
      }
    }
  }

  private static bool CanSecondaryLeak(int index)
  {
    for (int index1 = 0; index1 < cInput._inputLeaksSecondary[index].Count; ++index1)
    {
      if (Input.GetKey(cInput._inputLeaksSecondary[index][index1]))
        return false;
    }
    return true;
  }

  public static int SetAxis(string description, string negativeInput, string positiveInput)
  {
    return cInput.SetAxis(description, negativeInput, positiveInput, cInput.sensitivity, cInput.gravity, cInput.deadzone);
  }

  public static int SetAxis(
    string description,
    string negativeInput,
    string positiveInput,
    float axisSensitivity)
  {
    return cInput.SetAxis(description, negativeInput, positiveInput, axisSensitivity, cInput.gravity, cInput.deadzone);
  }

  public static int SetAxis(
    string description,
    string negativeInput,
    string positiveInput,
    float axisSensitivity,
    float axisGravity)
  {
    return cInput.SetAxis(description, negativeInput, positiveInput, axisSensitivity, axisGravity, cInput.deadzone);
  }

  public static int SetAxis(string description, string input)
  {
    return cInput.SetAxis(description, input, "-1", cInput.sensitivity, cInput.gravity, cInput.deadzone);
  }

  public static int SetAxis(string description, string input, float axisSensitivity)
  {
    return cInput.SetAxis(description, input, "-1", axisSensitivity, cInput.gravity, cInput.deadzone);
  }

  public static int SetAxis(
    string description,
    string input,
    float axisSensitivity,
    float axisGravity)
  {
    return cInput.SetAxis(description, input, "-1", axisSensitivity, axisGravity, cInput.deadzone);
  }

  public static int SetAxis(
    string description,
    string input,
    float axisSensitivity,
    float axisGravity,
    float axisDeadzone)
  {
    return cInput.SetAxis(description, input, "-1", axisSensitivity, axisGravity, axisDeadzone);
  }

  public static int SetAxis(
    string description,
    string negativeInput,
    string positiveInput,
    float axisSensitivity,
    float axisGravity,
    float axisDeadzone)
  {
    cInput._cInputInit();
    if (cInput.IsKeyDefined(negativeInput))
    {
      int _num = cInput._FindAxisByDescription(description);
      if (_num == -1)
        _num = cInput._axisLength + 1;
      int _positive = -1;
      int keyByDescription = cInput._FindKeyByDescription(negativeInput);
      if (cInput.IsKeyDefined(positiveInput))
      {
        _positive = cInput._FindKeyByDescription(positiveInput);
        cInput._markedAsAxis.Add(cInput._FindKeyByDescription(positiveInput));
        cInput._markedAsAxis.Add(keyByDescription);
      }
      else if (positiveInput != "-1")
      {
        Logger.ErrorFormat("Can't define Axis named: {0}. Please define '{1}' with SetKey() first.", (object) description, (object) positiveInput);
        return description.GetHashCode();
      }
      cInput._SetAxis(_num, description, keyByDescription, _positive);
      cInput._individualAxisSens[keyByDescription] = axisSensitivity;
      cInput._individualAxisGrav[keyByDescription] = axisGravity;
      cInput._individualAxisDead[keyByDescription] = axisDeadzone;
      cInput._defaultAxes[keyByDescription, 0] = axisSensitivity;
      cInput._defaultAxes[keyByDescription, 1] = axisGravity;
      cInput._defaultAxes[keyByDescription, 2] = axisDeadzone;
      if (_positive >= 0)
      {
        cInput._individualAxisSens[_positive] = axisSensitivity;
        cInput._individualAxisGrav[_positive] = axisGravity;
        cInput._individualAxisDead[_positive] = axisDeadzone;
        cInput._defaultAxes[_positive, 0] = axisSensitivity;
        cInput._defaultAxes[_positive, 1] = axisGravity;
        cInput._defaultAxes[_positive, 2] = axisDeadzone;
      }
    }
    else
      Logger.ErrorFormat("Can't define Axis named: {0}. Please define '{1}' with SetKey() first.", (object) description, (object) negativeInput);
    return description.GetHashCode();
  }

  private static void _SetAxis(int _num, string _description, int _negative, int _positive)
  {
    if (_num > cInput._axisLength)
      cInput._axisLength = _num;
    int hashCode = _description.GetHashCode();
    if (!cInput._axisNameHash.ContainsKey(hashCode))
      cInput._axisNameHash.Add(hashCode, _num);
    cInput._axisName[_num] = _description;
    cInput._makeAxis[_num, 0] = _negative;
    cInput._makeAxis[_num, 1] = _positive;
  }

  public static void SetAxisSensitivity(string axisName, float sensitivity)
  {
    cInput._SetAxisSensitivity(axisName.GetHashCode(), sensitivity, axisName);
  }

  public static void SetAxisSensitivity(int axisHash, float sensitivity)
  {
    cInput._SetAxisSensitivity(axisHash, sensitivity, string.Empty);
  }

  public static void SetAxisGravity(string axisName, float gravity)
  {
    cInput._SetAxisGravity(axisName.GetHashCode(), gravity, axisName);
  }

  public static void SetAxisGravity(int axisHash, float gravity)
  {
    cInput._SetAxisGravity(axisHash, gravity, string.Empty);
  }

  public static void SetAxisDeadzone(string axisName, float deadzone)
  {
    cInput._SetAxisDeadzone(axisName.GetHashCode(), deadzone, axisName);
  }

  public static void SetAxisDeadzone(int axisHash, float deadzone)
  {
    cInput._SetAxisDeadzone(axisHash, deadzone, string.Empty);
  }

  private static void _SetAxisSensitivity(int hash, float sensitivity, string description = "")
  {
    cInput._cInputInit();
    int axisByHash = cInput._FindAxisByHash(hash);
    if (axisByHash == -1)
    {
      Logger.ErrorFormat("Cannot set sensitivity of {0}. Have you defined this axis with SetAxis() yet?", (object) (string.IsNullOrEmpty(description) ? "axis matching hashcode of " + (object) hash : description));
    }
    else
    {
      cInput._individualAxisSens[cInput._makeAxis[axisByHash, 0]] = sensitivity;
      cInput._individualAxisSens[cInput._makeAxis[axisByHash, 1]] = sensitivity;
      cInput._SaveAxis();
    }
  }

  private static void _SetAxisGravity(int hash, float gravity, string description = "")
  {
    cInput._cInputInit();
    int axisByHash = cInput._FindAxisByHash(hash);
    if (axisByHash == -1)
    {
      Logger.ErrorFormat("Cannot set gravity of {0}. Have you defined this axis with SetAxis() yet?", (object) (string.IsNullOrEmpty(description) ? "axis matching hashcode of " + (object) hash : description));
    }
    else
    {
      cInput._individualAxisGrav[cInput._makeAxis[axisByHash, 0]] = gravity;
      cInput._individualAxisGrav[cInput._makeAxis[axisByHash, 1]] = gravity;
      cInput._SaveAxis();
    }
  }

  private static void _SetAxisDeadzone(int hash, float deadzone, string description = "")
  {
    cInput._cInputInit();
    int axisByHash = cInput._FindAxisByHash(hash);
    if (axisByHash == -1)
    {
      Logger.ErrorFormat("Cannot set deadzone of {0}. Have you defined this axis with SetAxis() yet?", (object) (string.IsNullOrEmpty(description) ? "axis matching hashcode of " + (object) hash : description));
    }
    else
    {
      cInput._individualAxisDead[cInput._makeAxis[axisByHash, 0]] = deadzone;
      cInput._individualAxisDead[cInput._makeAxis[axisByHash, 1]] = deadzone;
      cInput._SaveAxis();
    }
  }

  public static void Calibrate()
  {
    cInput._cInputInit();
    string str = string.Empty;
    cInput._axisCalibrationOffset = cInput._GetAxisRawValues();
    PlayerPrefs.SetString("cInput_calsVals", cInput._CalibrationValuesToString());
    for (int index1 = 1; index1 <= cInput._numGamepads; ++index1)
    {
      for (int index2 = 1; index2 <= 10; ++index2)
      {
        int index3 = 10 * (index1 - 1) + (index2 - 1);
        float axisRaw = Input.GetAxisRaw(cInput._joyStrings[index1, index2]);
        cInput._axisType[index3] = (double) axisRaw >= -(double) cInput.deadzone ? ((double) axisRaw <= (double) cInput.deadzone ? 0 : -1) : 1;
        str = str + (object) cInput._axisType[index3] + "*";
        PlayerPrefs.SetString("cInput_saveCals", str);
        cInput._exCalibrations = str;
      }
    }
  }

  private static string _CalibrationValuesToString()
  {
    string str = string.Empty;
    foreach (KeyValuePair<string, float> keyValuePair in cInput._axisCalibrationOffset)
      str = str + keyValuePair.Key + "*" + keyValuePair.Value.ToString() + "#";
    return str;
  }

  private static void _CalibrationValuesFromString(string calVals)
  {
    cInput._axisCalibrationOffset.Clear();
    string[] strArray1 = calVals.Split('#');
    for (int index = 0; index < strArray1.Length - 1; ++index)
    {
      string[] strArray2 = strArray1[index].Split('*');
      cInput._axisCalibrationOffset.Add(strArray2[0], float.Parse(strArray2[1]));
    }
  }

  private static float _GetCalibratedAxisInput(string description)
  {
    float axisRaw = Input.GetAxisRaw(cInput._ChangeStringToAxisName(description));
    string key = description;
    if (key != null)
    {
      // ISSUE: reference to a compiler-generated field
      if (cInput.\u003C\u003Ef__switch\u0024map33 == null)
      {
        // ISSUE: reference to a compiler-generated field
        cInput.\u003C\u003Ef__switch\u0024map33 = new Dictionary<string, int>(6)
        {
          {
            "Mouse Left",
            0
          },
          {
            "Mouse Right",
            0
          },
          {
            "Mouse Up",
            0
          },
          {
            "Mouse Down",
            0
          },
          {
            "Mouse Wheel Up",
            0
          },
          {
            "Mouse Wheel Down",
            0
          }
        };
      }
      int num;
      // ISSUE: reference to a compiler-generated field
      if (cInput.\u003C\u003Ef__switch\u0024map33.TryGetValue(key, out num) && num == 0)
        return axisRaw;
    }
    int[] numArray = (int[]) null;
    if (cInput._joyStringsPosIndices.ContainsKey(description))
      numArray = cInput._joyStringsPosIndices[description];
    else if (cInput._joyStringsNegIndices.ContainsKey(description))
      numArray = cInput._joyStringsNegIndices[description];
    if (numArray != null && numArray.Length > 0)
    {
      int index = 10 * (numArray[0] - 1) + (numArray[1] - 1);
      switch (cInput._axisType[index] + 1)
      {
        case 0:
          return (float) (((double) axisRaw - 1.0) / 2.0);
        case 2:
          return (float) (((double) axisRaw + 1.0) / 2.0);
        default:
          return axisRaw;
      }
    }
    else
    {
      Logger.WarningFormat("No match found for {0} ({1}). This should never happen, in theory. Returning value of {2}", (object) description, (object) cInput._ChangeStringToAxisName(description), (object) axisRaw);
      return axisRaw;
    }
  }

  public static void ChangeKey(
    string action,
    int input,
    bool allowMouseAxis,
    bool allowMouseButtons,
    bool allowGamepadAxis,
    bool allowGamepadButtons,
    bool allowKeyboard)
  {
    cInput._cInputInit();
    cInput.ChangeKey(cInput._FindKeyByDescription(action), input, allowMouseAxis, allowMouseButtons, allowGamepadAxis, allowGamepadButtons, allowKeyboard);
  }

  public static void ChangeKey(string action)
  {
    cInput.ChangeKey(action, 1, cInput._allowMouseAxis, cInput._allowMouseButtons, cInput._allowJoystickAxis, cInput._allowJoystickButtons, cInput._allowKeyboard);
  }

  public static void ChangeKey(string action, int input)
  {
    cInput.ChangeKey(action, input, cInput._allowMouseAxis, cInput._allowMouseButtons, cInput._allowJoystickAxis, cInput._allowJoystickButtons, cInput._allowKeyboard);
  }

  public static void ChangeKey(string action, int input, bool allowMouseAxis)
  {
    cInput.ChangeKey(action, input, allowMouseAxis, cInput._allowMouseButtons, cInput._allowJoystickAxis, cInput._allowJoystickButtons, cInput._allowKeyboard);
  }

  public static void ChangeKey(
    string action,
    int input,
    bool allowMouseAxis,
    bool allowMouseButtons)
  {
    cInput.ChangeKey(action, input, allowMouseAxis, allowMouseButtons, cInput._allowJoystickAxis, cInput._allowJoystickButtons, cInput._allowKeyboard);
  }

  public static void ChangeKey(
    string action,
    int input,
    bool allowMouseAxis,
    bool allowMouseButtons,
    bool allowGamepadAxis)
  {
    cInput.ChangeKey(action, input, allowMouseAxis, allowMouseButtons, allowGamepadAxis, cInput._allowJoystickButtons, cInput._allowKeyboard);
  }

  public static void ChangeKey(
    string action,
    int input,
    bool allowMouseAxis,
    bool allowMouseButtons,
    bool allowGamepadAxis,
    bool allowGamepadButtons)
  {
    cInput.ChangeKey(action, input, allowMouseAxis, allowMouseButtons, allowGamepadAxis, allowGamepadButtons, cInput._allowKeyboard);
  }

  public static void ChangeKey(
    int index,
    int input,
    bool allowMouseAxis,
    bool allowMouseButtons,
    bool allowGamepadAxis,
    bool allowGamepadButtons,
    bool allowKeyboard)
  {
    cInput._cInputInit();
    if (input != 1 && input != 2)
      Logger.WarningFormat("ChangeKey can only change primary (1) or secondary (2) inputs. You're trying to change: {0}", (object) input);
    else
      cInput._ScanForNewKey(index, input, allowMouseAxis, allowMouseButtons, allowGamepadAxis, allowGamepadButtons, allowKeyboard);
  }

  public static void ChangeKey(int index)
  {
    cInput.ChangeKey(index, 1, cInput._allowMouseAxis, cInput._allowMouseButtons, cInput._allowJoystickAxis, cInput._allowJoystickButtons, cInput._allowKeyboard);
  }

  public static void ChangeKey(int index, int input)
  {
    cInput.ChangeKey(index, input, cInput._allowMouseAxis, cInput._allowMouseButtons, cInput._allowJoystickAxis, cInput._allowJoystickButtons, cInput._allowKeyboard);
  }

  public static void ChangeKey(int index, int input, bool allowMouseAxis)
  {
    cInput.ChangeKey(index, input, allowMouseAxis, cInput._allowMouseButtons, cInput._allowJoystickAxis, cInput._allowJoystickButtons, cInput._allowKeyboard);
  }

  public static void ChangeKey(int index, int input, bool allowMouseAxis, bool allowMouseButtons)
  {
    cInput.ChangeKey(index, input, allowMouseAxis, allowMouseButtons, cInput._allowJoystickAxis, cInput._allowJoystickButtons, cInput._allowKeyboard);
  }

  public static void ChangeKey(
    int index,
    int input,
    bool allowMouseAxis,
    bool allowMouseButtons,
    bool allowGamepadAxis)
  {
    cInput.ChangeKey(index, input, allowMouseAxis, allowMouseButtons, allowGamepadAxis, cInput._allowJoystickButtons, cInput._allowKeyboard);
  }

  public static void ChangeKey(
    int index,
    int input,
    bool allowMouseAxis,
    bool allowMouseButtons,
    bool allowGamepadAxis,
    bool allowGamepadButtons)
  {
    cInput.ChangeKey(index, input, allowMouseAxis, allowMouseButtons, allowGamepadAxis, allowGamepadButtons, cInput._allowKeyboard);
  }

  public static void ChangeKey(
    string action,
    string primary,
    string secondary,
    string primaryModifier,
    string secondaryModifier)
  {
    cInput._cInputInit();
    int keyByDescription = cInput._FindKeyByDescription(action);
    if (string.IsNullOrEmpty(primaryModifier))
      primaryModifier = primary;
    if (string.IsNullOrEmpty(secondaryModifier))
      secondaryModifier = secondary;
    cInput._modifierUsedPrimary[keyByDescription] = cInput._ConvertString2Key(primaryModifier);
    cInput._modifierUsedSecondary[keyByDescription] = cInput._ConvertString2Key(secondaryModifier);
    cInput._ChangeKey(keyByDescription, action, primary, secondary);
  }

  public static void ChangeKey(string action, string primary)
  {
    int keyByDescription = cInput._FindKeyByDescription(action);
    cInput.ChangeKey(action, primary, string.Empty, primary, cInput._modifierUsedSecondary[keyByDescription].ToString());
  }

  public static void ChangeKey(string action, string primary, string secondary)
  {
    cInput.ChangeKey(action, primary, secondary, primary, secondary);
  }

  public static void ChangeKey(
    string action,
    string primary,
    string secondary,
    string primaryModifier)
  {
    cInput.ChangeKey(action, primary, secondary, primaryModifier, secondary);
  }

  public static void ChangeKey(
    string action,
    KeyCode primary,
    KeyCode secondary,
    KeyCode primaryModifier,
    KeyCode secondaryModifier)
  {
    cInput._cInputInit();
    int keyByDescription = cInput._FindKeyByDescription(action);
    cInput._modifierUsedPrimary[keyByDescription] = primaryModifier;
    cInput._modifierUsedSecondary[keyByDescription] = secondaryModifier;
    cInput._ChangeKey(keyByDescription, action, primary.ToString(), secondary.ToString());
  }

  public static void ChangeKey(
    string action,
    string primary,
    KeyCode secondary,
    KeyCode primaryModifier,
    KeyCode secondaryModifier)
  {
    cInput._cInputInit();
    int keyByDescription = cInput._FindKeyByDescription(action);
    cInput._modifierUsedPrimary[keyByDescription] = primaryModifier;
    cInput._modifierUsedSecondary[keyByDescription] = secondaryModifier;
    cInput._ChangeKey(keyByDescription, action, primary, secondary.ToString());
  }

  public static void ChangeKey(
    string action,
    KeyCode primary,
    string secondary,
    KeyCode primaryModifier,
    KeyCode secondaryModifier)
  {
    cInput._cInputInit();
    int keyByDescription = cInput._FindKeyByDescription(action);
    cInput._modifierUsedPrimary[keyByDescription] = primaryModifier;
    cInput._modifierUsedSecondary[keyByDescription] = secondaryModifier;
    cInput._ChangeKey(keyByDescription, action, primary.ToString(), secondary);
  }

  public static void ChangeKey(
    string action,
    string primary,
    string secondary,
    KeyCode primaryModifier,
    KeyCode secondaryModifier)
  {
    cInput._cInputInit();
    int keyByDescription = cInput._FindKeyByDescription(action);
    cInput._modifierUsedPrimary[keyByDescription] = primaryModifier;
    cInput._modifierUsedSecondary[keyByDescription] = secondaryModifier;
    cInput._ChangeKey(keyByDescription, action, primary, secondary);
  }

  public static void ChangeKey(string action, KeyCode primary)
  {
    int keyByDescription = cInput._FindKeyByDescription(action);
    cInput.ChangeKey(action, primary, cInput._inputSecondary[keyByDescription], primary, cInput._modifierUsedSecondary[keyByDescription]);
  }

  public static void ChangeKey(string action, KeyCode primary, KeyCode secondary)
  {
    cInput.ChangeKey(action, primary, secondary, primary, secondary);
  }

  public static void ChangeKey(
    string action,
    KeyCode primary,
    KeyCode secondary,
    KeyCode primaryModifier)
  {
    cInput.ChangeKey(action, primary, secondary, primaryModifier, secondary);
  }

  public static KeyCode GetActionKey(string action, bool isPrimary = true, bool isModifier = false)
  {
    return cInput.GetActionKey(cInput._FindKeyByDescription(action), isPrimary, isModifier);
  }

  public static KeyCode GetActionKey(int index, bool isPrimary = true, bool isModifier = false)
  {
    cInput._cInputInit();
    if (index < 0 || index > cInput._inputPrimary.Length)
      return KeyCode.None;
    return isPrimary ? (isModifier ? cInput._modifierUsedPrimary[index] : cInput._inputPrimary[index]) : (isModifier ? cInput._modifierUsedSecondary[index] : cInput._inputSecondary[index]);
  }

  public static string GetActionButton(string action, bool isPrimary = true, bool isModifier = false)
  {
    return cInput.GetActionButton(cInput._FindKeyByDescription(action), isPrimary, isModifier);
  }

  public static string GetActionButton(int index, bool isPrimary = true, bool isModifier = false)
  {
    cInput._cInputInit();
    if (index < 0 || index > cInput._inputPrimary.Length)
      return KeyCode.None.ToString();
    if (isPrimary)
    {
      if (isModifier)
        return cInput._modifierUsedPrimary[index].ToString();
      return string.IsNullOrEmpty(cInput._axisPrimary[index]) ? cInput._inputPrimary[index].ToString() : cInput._axisPrimary[index];
    }
    if (isModifier)
      return cInput._modifierUsedSecondary[index].ToString();
    return string.IsNullOrEmpty(cInput._axisSecondary[index]) ? cInput._inputSecondary[index].ToString() : cInput._axisSecondary[index];
  }

  private static void _ScanForNewKey(
    int index,
    int input,
    bool mouseAx,
    bool mouseBut,
    bool joyAx,
    bool joyBut,
    bool keyb)
  {
    cInput._allowMouseAxis = mouseAx;
    cInput._allowMouseButtons = mouseBut;
    cInput._allowJoystickButtons = joyBut;
    cInput._allowJoystickAxis = joyAx;
    cInput._allowKeyboard = keyb;
    cInput._cScanInput = input;
    cInput._cScanIndex = index;
    cInput._scanning = true;
    cInput._axisRawValues = cInput._GetAxisRawValues();
  }

  private static Dictionary<string, float> _GetAxisRawValues()
  {
    Dictionary<string, float> axisRawValues = new Dictionary<string, float>();
    axisRawValues.Add("Horizontal", Input.GetAxisRaw("Horizontal"));
    axisRawValues.Add("Vertical", Input.GetAxisRaw("Vertical"));
    axisRawValues.Add("Fire1", Input.GetAxisRaw("Fire1"));
    axisRawValues.Add("Fire2", Input.GetAxisRaw("Fire2"));
    axisRawValues.Add("Fire3", Input.GetAxisRaw("Fire3"));
    axisRawValues.Add("Jump", Input.GetAxisRaw("Jump"));
    axisRawValues.Add("Mouse X", Input.GetAxisRaw("Mouse X"));
    axisRawValues.Add("Mouse Y", Input.GetAxisRaw("Mouse Y"));
    axisRawValues.Add("Mouse Horizontal", Input.GetAxisRaw("Mouse Horizontal"));
    axisRawValues.Add("Mouse Vertical", Input.GetAxisRaw("Mouse Vertical"));
    axisRawValues.Add("Mouse ScrollWheel", Input.GetAxisRaw("Mouse ScrollWheel"));
    axisRawValues.Add("Mouse Wheel", Input.GetAxisRaw("Mouse Wheel"));
    axisRawValues.Add("Window Shake X", Input.GetAxisRaw("Window Shake X"));
    axisRawValues.Add("Window Shake Y", Input.GetAxisRaw("Window Shake Y"));
    axisRawValues.Add("Shift", Input.GetAxisRaw("Shift"));
    string empty = string.Empty;
    for (int index1 = 1; index1 <= cInput._numGamepads; ++index1)
    {
      for (int index2 = 1; index2 <= 10; ++index2)
      {
        string str = "Joy" + (object) index1 + " Axis " + (object) index2;
        axisRawValues.Add(str, Input.GetAxis(str));
      }
    }
    return axisRawValues;
  }

  private static void _ChangeKey(int num, string action, string primary, string secondary)
  {
    cInput._SetKey(num, action, primary, secondary);
    cInput._SaveInputs();
  }

  private static bool _DefaultsExist() => cInput._defaultStrings.Length > 0;

  private static bool _IsKeyDefined(int hash)
  {
    cInput._cInputInit();
    return cInput._inputNameHash.ContainsKey(hash);
  }

  public static bool IsKeyDefined(string keyName) => cInput._IsKeyDefined(keyName.GetHashCode());

  public static bool IsKeyDefined(int keyHash) => cInput._IsKeyDefined(keyHash);

  private static bool _IsAxisDefined(int hash)
  {
    cInput._cInputInit();
    return cInput._axisNameHash.ContainsKey(hash);
  }

  public static bool IsAxisDefined(string axisName)
  {
    return cInput._IsAxisDefined(axisName.GetHashCode());
  }

  public static bool IsAxisDefined(int axisHash) => cInput._IsAxisDefined(axisHash);

  private void _CheckInputs()
  {
    cInput.anyKey = false;
    cInput.anyKeyUp = false;
    cInput.anyKeyDown = false;
    for (int index1 = 0; index1 < cInput._inputLength + 1; ++index1)
    {
      bool key1 = Input.GetKey(cInput._inputPrimary[index1]);
      bool key2 = Input.GetKey(cInput._inputSecondary[index1]);
      bool flag1 = false;
      bool flag2 = false;
      bool flag3 = false;
      for (int index2 = 0; index2 < cInput._modifiers.Count; ++index2)
      {
        if (Input.GetKey(cInput._modifiers[index2]))
        {
          flag3 = true;
          if (!flag1 && cInput._modifiers[index2] == cInput._modifierUsedPrimary[index1])
            flag1 = true;
          if (!flag2 && cInput._modifiers[index2] == cInput._modifierUsedSecondary[index1])
            flag2 = true;
        }
      }
      bool flag4 = cInput._modifierUsedPrimary[index1] == cInput._inputPrimary[index1] && (!flag3 || cInput.CanPrimaryLeak(index1)) || cInput._modifierUsedPrimary[index1] != cInput._inputPrimary[index1] && flag1;
      bool flag5 = cInput._modifierUsedSecondary[index1] == cInput._inputSecondary[index1] && (!flag3 || cInput.CanSecondaryLeak(index1)) || cInput._modifierUsedSecondary[index1] != cInput._inputSecondary[index1] && flag2;
      bool flag6;
      float a;
      if (!string.IsNullOrEmpty(cInput._axisPrimary[index1]))
      {
        flag6 = true;
        a = !flag4 ? 0.0f : cInput._GetCalibratedAxisInput(cInput._axisPrimary[index1]) * (float) cInput._PosOrNeg(cInput._axisPrimary[index1]);
      }
      else
      {
        flag6 = false;
        a = !key1 ? 0.0f : 1f;
      }
      bool flag7;
      float b;
      if (!string.IsNullOrEmpty(cInput._axisSecondary[index1]))
      {
        flag7 = true;
        b = !flag5 ? 0.0f : cInput._GetCalibratedAxisInput(cInput._axisSecondary[index1]) * (float) cInput._PosOrNeg(cInput._axisSecondary[index1]);
      }
      else
      {
        flag7 = false;
        b = !key2 ? 0.0f : 1f;
      }
      if (key1 && flag4 || key2 && flag5 || flag6 && flag4 && (double) a > (double) cInput.deadzone || flag7 && flag5 && (double) b > (double) cInput.deadzone)
      {
        cInput._getKeyArray[index1] = true;
        cInput.anyKey = true;
      }
      else
        cInput._getKeyArray[index1] = false;
      if (flag4 && Input.GetKeyDown(cInput._inputPrimary[index1]) || flag5 && Input.GetKeyDown(cInput._inputSecondary[index1]))
      {
        cInput._getKeyDownArray[index1] = true;
        cInput.anyKeyDown = true;
      }
      else
      {
        bool flag8 = false;
        if (flag6 && flag4 && (double) a > (double) cInput.deadzone && !cInput._axisTriggerArrayPrimary[index1])
        {
          cInput._axisTriggerArrayPrimary[index1] = true;
          cInput.anyKeyDown = true;
          flag8 = true;
        }
        if (flag7 && flag5 && (double) b > (double) cInput.deadzone && !cInput._axisTriggerArraySecondary[index1])
        {
          cInput._axisTriggerArraySecondary[index1] = true;
          cInput.anyKeyDown = true;
          flag8 = true;
        }
        cInput._getKeyDownArray[index1] = (cInput._axisTriggerArrayPrimary[index1] || cInput._axisTriggerArraySecondary[index1]) && flag8;
      }
      if (Input.GetKeyUp(cInput._inputPrimary[index1]) && flag4 || Input.GetKeyUp(cInput._inputSecondary[index1]) && flag5)
      {
        cInput._getKeyUpArray[index1] = true;
        cInput.anyKeyUp = true;
      }
      else
      {
        bool flag9 = false;
        if (flag6 && flag4 && (double) a <= (double) cInput.deadzone && cInput._axisTriggerArrayPrimary[index1])
        {
          cInput._axisTriggerArrayPrimary[index1] = false;
          cInput.anyKeyUp = true;
          flag9 = true;
        }
        if (flag7 && flag5 && (double) b <= (double) cInput.deadzone && cInput._axisTriggerArraySecondary[index1])
        {
          cInput._axisTriggerArraySecondary[index1] = false;
          cInput.anyKeyUp = true;
          flag9 = true;
        }
        cInput._getKeyUpArray[index1] = (!cInput._axisTriggerArrayPrimary[index1] || !cInput._axisTriggerArraySecondary[index1]) && flag9;
      }
      float sensitivity = cInput.sensitivity;
      float gravity = cInput.gravity;
      float deadzone = cInput.deadzone;
      cInput.sensitivity = (double) cInput._individualAxisSens[index1] == -99.0 ? sensitivity : cInput._individualAxisSens[index1];
      cInput.gravity = (double) cInput._individualAxisGrav[index1] == -99.0 ? gravity : cInput._individualAxisGrav[index1];
      cInput.deadzone = (double) cInput._individualAxisDead[index1] == -99.0 ? deadzone : cInput._individualAxisDead[index1];
      float num = (double) Time.deltaTime != 0.0 ? Time.deltaTime : 0.012f;
      if ((double) a > (double) cInput.deadzone || (double) b > (double) cInput.deadzone)
      {
        cInput._getAxisRaw[index1] = Mathf.Max(a, b);
        if ((double) cInput._getAxis[index1] < (double) cInput._getAxisRaw[index1])
          cInput._getAxis[index1] = Mathf.Min(cInput._getAxis[index1] + cInput.sensitivity * num, cInput._getAxisRaw[index1]);
        if ((double) cInput._getAxis[index1] > (double) cInput._getAxisRaw[index1])
          cInput._getAxis[index1] = Mathf.Max(cInput._getAxisRaw[index1], cInput._getAxis[index1] - cInput.gravity * num);
      }
      else
      {
        cInput._getAxisRaw[index1] = 0.0f;
        if ((double) cInput._getAxis[index1] > 0.0)
          cInput._getAxis[index1] = Mathf.Max(0.0f, cInput._getAxis[index1] - cInput.gravity * num);
      }
      cInput.sensitivity = sensitivity;
      cInput.gravity = gravity;
      cInput.deadzone = deadzone;
    }
    for (int index = 0; index <= cInput._axisLength; ++index)
    {
      int makeAxi1 = cInput._makeAxis[index, 0];
      int makeAxi2 = cInput._makeAxis[index, 1];
      if (cInput._makeAxis[index, 1] == -1)
      {
        cInput._getAxisArray[index] = cInput._getAxis[makeAxi1];
        cInput._getAxisArrayRaw[index] = cInput._getAxisRaw[makeAxi1];
      }
      else
      {
        cInput._getAxisArray[index] = cInput._getAxis[makeAxi2] - cInput._getAxis[makeAxi1];
        cInput._getAxisArrayRaw[index] = cInput._getAxisRaw[makeAxi2] - cInput._getAxisRaw[makeAxi1];
      }
    }
  }

  private static int _FindKeyByHash(int hash)
  {
    return cInput._inputNameHash.ContainsKey(hash) ? cInput._inputNameHash[hash] : -1;
  }

  private static int _FindKeyByDescription(string description)
  {
    return cInput._FindKeyByHash(description.GetHashCode());
  }

  private static bool _GetKey(int hash, string description = "")
  {
    cInput._cInputInit();
    if (!cInput._DefaultsExist())
    {
      Logger.Error("No default inputs found. Please setup your default inputs with SetKey first.");
      return false;
    }
    if (!cInput._cKeysLoaded)
      return false;
    int keyByHash = cInput._FindKeyByHash(hash);
    if (keyByHash > -1)
      return cInput._getKeyArray[keyByHash];
    Logger.ErrorFormat("Couldn't find a key match for {0}. Is it possible you typed it wrong or forgot to setup your defaults after making changes?", (object) (string.IsNullOrEmpty(description) ? "hash " + (object) hash : description));
    return false;
  }

  public static bool GetKey(string description)
  {
    return cInput._GetKey(description.GetHashCode(), description);
  }

  public static bool GetKey(int descriptionHash) => cInput._GetKey(descriptionHash, string.Empty);

  private static bool _GetKeyDown(int hash, string description = "")
  {
    cInput._cInputInit();
    if (!cInput._DefaultsExist())
    {
      Logger.Error("No default inputs found. Please setup your default inputs with SetKey first.");
      return false;
    }
    if (!cInput._cKeysLoaded)
      return false;
    int keyByHash = cInput._FindKeyByHash(hash);
    if (keyByHash > -1)
      return cInput._getKeyDownArray[keyByHash];
    Logger.ErrorFormat("Couldn't find a key match for {0}. Is it possible you typed it wrong or forgot to setup your defaults after making changes?", (object) (string.IsNullOrEmpty(description) ? "hash " + (object) hash : description));
    return false;
  }

  public static bool GetKeyDown(string description)
  {
    return cInput._GetKeyDown(description.GetHashCode(), description);
  }

  public static bool GetKeyDown(int descriptionHash)
  {
    return cInput._GetKeyDown(descriptionHash, string.Empty);
  }

  private static bool _GetKeyUp(int hash, string description = "")
  {
    cInput._cInputInit();
    if (!cInput._DefaultsExist())
    {
      Logger.Error("No default inputs found. Please setup your default inputs with SetKey first.");
      return false;
    }
    if (!cInput._cKeysLoaded)
      return false;
    int keyByHash = cInput._FindKeyByHash(hash);
    if (keyByHash > -1)
      return cInput._getKeyUpArray[keyByHash];
    Logger.ErrorFormat("Couldn't find a key match for {0}. Is it possible you typed it wrong or forgot to setup your defaults after making changes?", (object) (string.IsNullOrEmpty(description) ? "hash " + (object) hash : description));
    return false;
  }

  public static bool GetKeyUp(string description)
  {
    return cInput._GetKeyUp(description.GetHashCode(), description);
  }

  public static bool GetKeyUp(int descriptionHash)
  {
    return cInput._GetKeyUp(descriptionHash, string.Empty);
  }

  public static bool GetButton(string description) => cInput.GetKey(description);

  public static bool GetButton(int descriptionHash) => cInput.GetKey(descriptionHash);

  public static bool GetButtonDown(string description) => cInput.GetKeyDown(description);

  public static bool GetButtonDown(int descriptionHash) => cInput.GetKeyDown(descriptionHash);

  public static bool GetButtonUp(string description) => cInput.GetKeyUp(description);

  public static bool GetButtonUp(int descriptionHash) => cInput.GetKeyUp(descriptionHash);

  private static int _FindAxisByHash(int hash)
  {
    return cInput._axisNameHash.ContainsKey(hash) ? cInput._axisNameHash[hash] : -1;
  }

  private static int _FindAxisByDescription(string description)
  {
    return cInput._FindAxisByHash(description.GetHashCode());
  }

  private static float _GetAxis(int hash, string description = "")
  {
    cInput._cInputInit();
    if (!cInput._DefaultsExist())
    {
      Logger.Error("No default inputs found. Please setup your default inputs with SetKey first.");
      return 0.0f;
    }
    int axisByHash = cInput._FindAxisByHash(hash);
    if (axisByHash > -1)
      return cInput._invertAxis[axisByHash] ? cInput._getAxisArray[axisByHash] * -1f : cInput._getAxisArray[axisByHash];
    Logger.ErrorFormat("Couldn't find an axis match for {0}. Is it possible you typed it wrong?", (object) (string.IsNullOrEmpty(description) ? "axis with hashcode of " + (object) hash : description));
    return 0.0f;
  }

  public static float GetAxis(string description)
  {
    return cInput._GetAxis(description.GetHashCode(), description);
  }

  public static float GetAxis(int descriptionHash)
  {
    return cInput._GetAxis(descriptionHash, string.Empty);
  }

  private static float _GetAxisRaw(int hash, string description = "")
  {
    cInput._cInputInit();
    if (!cInput._DefaultsExist())
    {
      Logger.Error("No default inputs found. Please setup your default inputs with SetKey first.");
      return 0.0f;
    }
    int axisByHash = cInput._FindAxisByHash(hash);
    if (axisByHash > -1)
      return cInput._invertAxis[axisByHash] ? cInput._getAxisArrayRaw[axisByHash] * -1f : cInput._getAxisArrayRaw[axisByHash];
    Logger.ErrorFormat("Couldn't find an axis match for {0}. Is it possible you typed it wrong?", (object) (string.IsNullOrEmpty(description) ? "axis with hashcode of " + (object) hash : description));
    return 0.0f;
  }

  public static float GetAxisRaw(string description)
  {
    return cInput._GetAxisRaw(description.GetHashCode(), description);
  }

  public static float GetAxisRaw(int descriptionHash)
  {
    return cInput._GetAxisRaw(descriptionHash, string.Empty);
  }

  private static float _GetAxisSensitivity(int hash, string description = "")
  {
    cInput._cInputInit();
    int axisByHash = cInput._FindAxisByHash(hash);
    if (axisByHash != -1)
      return cInput._individualAxisSens[cInput._makeAxis[axisByHash, 0]];
    Logger.ErrorFormat("Cannot get sensitivity of {0}. Have you defined this axis with SetAxis() yet?", (object) (string.IsNullOrEmpty(description) ? "axis with hashcode of " + (object) hash : description));
    return -1f;
  }

  public static float GetAxisSensitivity(string description)
  {
    return cInput._GetAxisSensitivity(description.GetHashCode(), description);
  }

  public static float GetAxisSensitivity(int descriptionHash)
  {
    return cInput._GetAxisSensitivity(descriptionHash, string.Empty);
  }

  private static float _GetAxisGravity(int hash, string description = "")
  {
    cInput._cInputInit();
    int axisByHash = cInput._FindAxisByHash(hash);
    if (axisByHash != -1)
      return cInput._individualAxisGrav[cInput._makeAxis[axisByHash, 0]];
    Logger.ErrorFormat("Cannot get gravity of {0}. Have you defined this axis with SetAxis() yet?", (object) (string.IsNullOrEmpty(description) ? "axis with hashcode of " + (object) hash : description));
    return -1f;
  }

  public static float GetAxisGravity(string description)
  {
    return cInput._GetAxisGravity(description.GetHashCode(), description);
  }

  public static float GetAxisGravity(int descriptionHash)
  {
    return cInput._GetAxisGravity(descriptionHash, string.Empty);
  }

  private static float _GetAxisDeadzone(int hash, string description = "")
  {
    cInput._cInputInit();
    int axisByHash = cInput._FindAxisByHash(hash);
    if (axisByHash != -1)
      return cInput._individualAxisDead[cInput._makeAxis[axisByHash, 0]];
    Logger.ErrorFormat("Cannot get deadzone of {0}. Have you defined this axis with SetAxis() yet?", (object) (string.IsNullOrEmpty(description) ? "axis with hashcode of " + (object) hash : description));
    return -1f;
  }

  public static float GetAxisDeadzone(string description)
  {
    return cInput._GetAxisDeadzone(description.GetHashCode(), description);
  }

  public static float GetAxisDeadzone(int descriptionHash)
  {
    return cInput._GetAxisDeadzone(descriptionHash, string.Empty);
  }

  public static string GetText(string action) => cInput.GetText(action, 1);

  public static string GetText(int index) => cInput.GetText(index, 0);

  public static string GetText(string action, int input)
  {
    return cInput.GetText(cInput._FindKeyByDescription(action), input);
  }

  public static string GetText(string action, int input, bool returnBlank)
  {
    string text = cInput.GetText(cInput._FindKeyByDescription(action), input);
    if (returnBlank && text == "None")
      text = string.Empty;
    return text;
  }

  public static string GetText(int index, int input, bool returnBlank)
  {
    string text = cInput.GetText(index, input);
    if (returnBlank && text == "None")
      text = string.Empty;
    return text;
  }

  public static string GetText(int index, int input)
  {
    cInput._cInputInit();
    if (input < 0 || input > 2)
    {
      Logger.WarningFormat("Can't look for text #{0} for {1} input. Only 0, 1, or 2 is acceptable. Clamping to this range.", (object) input, (object) cInput._inputName[index]);
      input = Mathf.Clamp(input, 0, 2);
    }
    string text = string.Empty;
    switch (input)
    {
      case 1:
        string str1 = string.Empty;
        if (cInput._modifierUsedPrimary[index] != KeyCode.None && cInput._modifierUsedPrimary[index] != cInput._inputPrimary[index])
          str1 = cInput._modifierUsedPrimary[index].ToString() + " + ";
        if (!string.IsNullOrEmpty(cInput._axisPrimary[index]))
        {
          text = str1 + cInput._axisPrimary[index];
          break;
        }
        if (cInput._inputPrimary[index] != KeyCode.None)
        {
          text = str1 + cInput._inputPrimary[index].ToString();
          break;
        }
        break;
      case 2:
        string str2 = string.Empty;
        if (cInput._modifierUsedSecondary[index] != KeyCode.None && cInput._modifierUsedSecondary[index] != cInput._inputSecondary[index])
          str2 = cInput._modifierUsedSecondary[index].ToString() + " + ";
        if (!string.IsNullOrEmpty(cInput._axisSecondary[index]))
        {
          text = str2 + cInput._axisSecondary[index];
          break;
        }
        if (cInput._inputSecondary[index] != KeyCode.None)
        {
          text = str2 + cInput._inputSecondary[index].ToString();
          break;
        }
        break;
      default:
        return cInput._inputName[index];
    }
    if (cInput._scanning && index == cInput._cScanIndex && input == cInput._cScanInput)
      text = ". . .";
    return text;
  }

  private static string _ChangeStringToAxisName(string description)
  {
    string key = description;
    if (key != null)
    {
      // ISSUE: reference to a compiler-generated field
      if (cInput.\u003C\u003Ef__switch\u0024map34 == null)
      {
        // ISSUE: reference to a compiler-generated field
        cInput.\u003C\u003Ef__switch\u0024map34 = new Dictionary<string, int>(6)
        {
          {
            "Mouse Left",
            0
          },
          {
            "Mouse Right",
            1
          },
          {
            "Mouse Up",
            2
          },
          {
            "Mouse Down",
            3
          },
          {
            "Mouse Wheel Up",
            4
          },
          {
            "Mouse Wheel Down",
            5
          }
        };
      }
      int num;
      // ISSUE: reference to a compiler-generated field
      if (cInput.\u003C\u003Ef__switch\u0024map34.TryGetValue(key, out num))
      {
        switch (num)
        {
          case 0:
            return "Mouse Horizontal";
          case 1:
            return "Mouse Horizontal";
          case 2:
            return "Mouse Vertical";
          case 3:
            return "Mouse Vertical";
          case 4:
            return "Mouse Wheel";
          case 5:
            return "Mouse Wheel";
        }
      }
    }
    string joystringByDescription = cInput._FindJoystringByDescription(description);
    return !string.IsNullOrEmpty(joystringByDescription) ? joystringByDescription : description;
  }

  private static string _FindJoystringByDescription(string description)
  {
    if (cInput._joyStringsPosIndices.ContainsKey(description))
    {
      int[] joyStringsPosIndex = cInput._joyStringsPosIndices[description];
      return cInput._joyStrings[joyStringsPosIndex[0], joyStringsPosIndex[1]];
    }
    if (!cInput._joyStringsNegIndices.ContainsKey(description))
      return (string) null;
    int[] joyStringsNegIndex = cInput._joyStringsNegIndices[description];
    return cInput._joyStrings[joyStringsNegIndex[0], joyStringsNegIndex[1]];
  }

  private static bool _IsAxisValid(string axis)
  {
    string key = axis;
    if (key != null)
    {
      // ISSUE: reference to a compiler-generated field
      if (cInput.\u003C\u003Ef__switch\u0024map35 == null)
      {
        // ISSUE: reference to a compiler-generated field
        cInput.\u003C\u003Ef__switch\u0024map35 = new Dictionary<string, int>(6)
        {
          {
            "Mouse Left",
            0
          },
          {
            "Mouse Right",
            0
          },
          {
            "Mouse Up",
            0
          },
          {
            "Mouse Down",
            0
          },
          {
            "Mouse Wheel Up",
            0
          },
          {
            "Mouse Wheel Down",
            0
          }
        };
      }
      int num;
      // ISSUE: reference to a compiler-generated field
      if (cInput.\u003C\u003Ef__switch\u0024map35.TryGetValue(key, out num) && num == 0)
        return true;
    }
    return cInput._joyStringsPosIndices.ContainsKey(axis) || cInput._joyStringsNegIndices.ContainsKey(axis);
  }

  private static int _PosOrNeg(string description)
  {
    int num1 = 1;
    string key = description;
    if (key != null)
    {
      // ISSUE: reference to a compiler-generated field
      if (cInput.\u003C\u003Ef__switch\u0024map36 == null)
      {
        // ISSUE: reference to a compiler-generated field
        cInput.\u003C\u003Ef__switch\u0024map36 = new Dictionary<string, int>(3)
        {
          {
            "Mouse Left",
            0
          },
          {
            "Mouse Down",
            1
          },
          {
            "Mouse Wheel Down",
            2
          }
        };
      }
      int num2;
      // ISSUE: reference to a compiler-generated field
      if (cInput.\u003C\u003Ef__switch\u0024map36.TryGetValue(key, out num2))
      {
        switch (num2)
        {
          case 0:
            return -1;
          case 1:
            return -1;
          case 2:
            return -1;
        }
      }
    }
    return cInput._joyStringsNegIndices.ContainsKey(description) ? -1 : num1;
  }

  private static void _SaveModifier()
  {
    string str = cInput._modifiers.Count <= 0 ? string.Empty : ((int) cInput._modifiers[0]).ToString();
    for (int index = 1; index < cInput._modifiers.Count; ++index)
      str = str + "*" + (object) (int) cInput._modifiers[index];
    PlayerPrefs.SetString("cInput_modifiers", str);
  }

  private static void _SaveAxis()
  {
    int num = cInput._axisLength + 1;
    string str1 = string.Empty;
    string str2 = string.Empty;
    string str3 = string.Empty;
    string str4 = string.Empty;
    string str5 = string.Empty;
    string str6 = string.Empty;
    for (int index = 0; index < num; ++index)
    {
      int axisByDescription = cInput._FindAxisByDescription(cInput._axisName[index]);
      str1 = str1 + cInput._axisName[index] + "*";
      str2 = str2 + (object) cInput._makeAxis[axisByDescription, 0] + "*";
      str3 = str3 + (object) cInput._makeAxis[axisByDescription, 1] + "*";
      str4 = str4 + (object) cInput._individualAxisSens[cInput._makeAxis[axisByDescription, 0]] + "*";
      str5 = str5 + (object) cInput._individualAxisGrav[cInput._makeAxis[axisByDescription, 0]] + "*";
      str6 = str6 + (object) cInput._individualAxisDead[cInput._makeAxis[axisByDescription, 0]] + "*";
    }
    string str7 = str1 + "#" + str2 + "#" + str3 + "#" + (object) num;
    PlayerPrefs.SetString("cInput_axis", str7);
    PlayerPrefs.SetString("cInput_indAxSens", str4);
    PlayerPrefs.SetString("cInput_indAxGrav", str5);
    PlayerPrefs.SetString("cInput_indAxDead", str6);
    cInput._exAxis = str7 + "¿" + str4 + "¿" + str5 + "¿" + str6;
  }

  private static void _SaveAxInverted()
  {
    int num = cInput._axisLength + 1;
    string str = string.Empty;
    for (int index = 0; index < num; ++index)
      str = str + (object) cInput._invertAxis[index] + "*";
    PlayerPrefs.SetString("cInput_axInv", str);
    cInput._exAxisInverted = str;
  }

  private static void _SaveDefaults()
  {
    int num = cInput._inputLength + 1;
    string str1 = string.Empty;
    string str2 = string.Empty;
    string str3 = string.Empty;
    string str4 = string.Empty;
    string str5 = string.Empty;
    for (int index = 0; index < num; ++index)
    {
      str1 = str1 + cInput._defaultStrings[index, 0] + "*";
      str2 = str2 + cInput._defaultStrings[index, 1] + "*";
      str3 = str3 + cInput._defaultStrings[index, 2] + "*";
      str4 = str4 + cInput._defaultStrings[index, 3] + "*";
      str5 = str5 + cInput._defaultStrings[index, 4] + "*";
    }
    string str6 = str1 + "#" + str2 + "#" + str3 + "#" + str4 + "#" + str5;
    PlayerPrefs.SetInt("cInput_count", num);
    PlayerPrefs.SetString("cInput_defaults", str6);
    cInput._exDefaults = num.ToString() + "¿" + str6;
  }

  private static void _SaveInputs()
  {
    int num = cInput._inputLength + 1;
    string str1 = string.Empty;
    string str2 = string.Empty;
    string str3 = string.Empty;
    string str4 = string.Empty;
    string str5 = string.Empty;
    string str6 = string.Empty;
    string str7 = string.Empty;
    for (int index = 0; index < num; ++index)
    {
      str1 = str1 + cInput._inputName[index] + "*";
      str2 = str2 + (object) cInput._inputPrimary[index] + "*";
      str3 = str3 + (object) cInput._inputSecondary[index] + "*";
      str4 = str4 + cInput._axisPrimary[index] + "*";
      str5 = str5 + cInput._axisSecondary[index] + "*";
      str6 = str6 + (object) cInput._modifierUsedPrimary[index] + "*";
      str7 = str7 + (object) cInput._modifierUsedSecondary[index] + "*";
    }
    PlayerPrefs.SetString("cInput_descr", str1);
    PlayerPrefs.SetString("cInput_inp", str2);
    PlayerPrefs.SetString("cInput_alt_inp", str3);
    PlayerPrefs.SetString("cInput_inpStr", str4);
    PlayerPrefs.SetString("cInput_alt_inpStr", str5);
    PlayerPrefs.SetString("cInput_modifierStr", str6);
    PlayerPrefs.SetString("cInput_alt_modifierStr", str7);
    cInput._exInputs = str1 + "¿" + str2 + "¿" + str3 + "¿" + str4 + "¿" + str5 + "¿" + str6 + "¿" + str7;
  }

  public static string externalInputs
  {
    get
    {
      return cInput._exAllowDuplicates + "æ" + cInput._exAxis + "æ" + cInput._exAxisInverted + "æ" + cInput._exDefaults + "æ" + cInput._exInputs + "æ" + cInput._exCalibrations + "æ" + cInput._exCalibrationValues;
    }
  }

  public static void LoadExternal(string externString)
  {
    cInput._cInputInit();
    string[] strArray = externString.Split('æ');
    cInput._exAllowDuplicates = strArray[0];
    cInput._exAxis = strArray[1];
    cInput._exAxisInverted = strArray[2];
    cInput._exDefaults = strArray[3];
    cInput._exInputs = strArray[4];
    cInput._exCalibrations = strArray[5];
    cInput._exCalibrationValues = strArray[6];
    cInput._LoadExternalInputs();
  }

  private static void _LoadInputs()
  {
    if (!PlayerPrefs.HasKey("cInput_count"))
    {
      cInput._cKeysLoaded = true;
    }
    else
    {
      if (PlayerPrefs.HasKey("cInput_dubl"))
        cInput.allowDuplicates = PlayerPrefs.GetString("cInput_dubl") == "True";
      if (PlayerPrefs.HasKey("cInput_modifiers"))
      {
        cInput._modifiers.Clear();
        string str = PlayerPrefs.GetString("cInput_modifiers");
        char[] chArray = new char[1]{ '*' };
        foreach (string s in str.Split(chArray))
        {
          int result;
          if (int.TryParse(s, out result))
          {
            KeyCode keyCode = (KeyCode) result;
            if (!cInput._modifiers.Contains(keyCode))
              cInput._modifiers.Add(keyCode);
          }
        }
      }
      cInput._inputLength = PlayerPrefs.GetInt("cInput_count") - 1;
      string[] strArray1 = PlayerPrefs.GetString("cInput_defaults").Split('#');
      string[] strArray2 = strArray1[0].Split('*');
      string[] strArray3 = strArray1[1].Split('*');
      string[] strArray4 = strArray1[2].Split('*');
      string[] strArray5 = strArray1[3].Split('*');
      string[] strArray6 = strArray1[4].Split('*');
      for (int _num = 0; _num < strArray2.Length - 1; ++_num)
        cInput._SetDefaultKey(_num, strArray2[_num], strArray3[_num], strArray4[_num], strArray5[_num], strArray6[_num]);
      if (PlayerPrefs.HasKey("cInput_inp"))
      {
        string str1 = PlayerPrefs.GetString("cInput_descr");
        string str2 = PlayerPrefs.GetString("cInput_inp");
        string str3 = PlayerPrefs.GetString("cInput_alt_inp");
        string str4 = PlayerPrefs.GetString("cInput_inpStr");
        string str5 = PlayerPrefs.GetString("cInput_alt_inpStr");
        string str6 = PlayerPrefs.GetString("cInput_modifierStr");
        string str7 = PlayerPrefs.GetString("cInput_alt_modifierStr");
        string[] strArray7 = str1.Split('*');
        string[] strArray8 = str2.Split('*');
        string[] strArray9 = str3.Split('*');
        string[] strArray10 = str4.Split('*');
        string[] strArray11 = str5.Split('*');
        string[] strArray12 = str6.Split('*');
        string[] strArray13 = str7.Split('*');
        for (int index = 0; index < strArray7.Length - 1; ++index)
        {
          if (strArray7[index] == cInput._defaultStrings[index, 0])
          {
            cInput._inputName[index] = strArray7[index];
            cInput._inputPrimary[index] = cInput._ConvertString2Key(strArray8[index]);
            cInput._inputSecondary[index] = cInput._ConvertString2Key(strArray9[index]);
            cInput._axisPrimary[index] = strArray10[index];
            cInput._axisSecondary[index] = strArray11[index];
            cInput._modifierUsedPrimary[index] = cInput._ConvertString2Key(strArray12[index]);
            cInput._modifierUsedSecondary[index] = cInput._ConvertString2Key(strArray13[index]);
          }
        }
        for (int index1 = 0; index1 < strArray2.Length - 1; ++index1)
        {
          for (int index2 = 0; index2 < strArray7.Length - 1; ++index2)
          {
            if (strArray7[index2] == cInput._defaultStrings[index1, 0])
            {
              cInput._inputName[index1] = strArray7[index2];
              cInput._inputPrimary[index1] = cInput._ConvertString2Key(strArray8[index2]);
              cInput._inputSecondary[index1] = cInput._ConvertString2Key(strArray9[index2]);
              cInput._axisPrimary[index1] = strArray10[index2];
              cInput._axisSecondary[index1] = strArray11[index2];
              cInput._modifierUsedPrimary[index2] = cInput._ConvertString2Key(strArray12[index2]);
              cInput._modifierUsedSecondary[index2] = cInput._ConvertString2Key(strArray13[index2]);
            }
          }
        }
      }
      if (PlayerPrefs.HasKey("cInput_axis"))
      {
        string[] strArray14 = PlayerPrefs.GetString("cInput_axis").Split('#');
        string[] strArray15 = strArray14[0].Split('*');
        string[] strArray16 = strArray14[1].Split('*');
        string[] strArray17 = strArray14[2].Split('*');
        int num = int.Parse(strArray14[3]);
        for (int _num = 0; _num < num; ++_num)
        {
          int _negative = int.Parse(strArray16[_num]);
          int _positive = int.Parse(strArray17[_num]);
          cInput._SetAxis(_num, strArray15[_num], _negative, _positive);
        }
      }
      if (PlayerPrefs.HasKey("cInput_axInv"))
      {
        string[] strArray18 = PlayerPrefs.GetString("cInput_axInv").Split('*');
        for (int index = 0; index < strArray18.Length; ++index)
          cInput._invertAxis[index] = strArray18[index] == "True";
      }
      if (PlayerPrefs.HasKey("cInput_indAxSens"))
      {
        string[] strArray19 = PlayerPrefs.GetString("cInput_indAxSens").Split('*');
        for (int index = 0; index < strArray19.Length - 1; ++index)
        {
          int axisByDescription = cInput._FindAxisByDescription(cInput._axisName[index]);
          cInput._individualAxisSens[cInput._makeAxis[axisByDescription, 0]] = float.Parse(strArray19[index]);
          cInput._individualAxisSens[cInput._makeAxis[axisByDescription, 1]] = float.Parse(strArray19[index]);
        }
      }
      if (PlayerPrefs.HasKey("cInput_indAxGrav"))
      {
        string[] strArray20 = PlayerPrefs.GetString("cInput_indAxGrav").Split('*');
        for (int index = 0; index < strArray20.Length - 1; ++index)
        {
          int axisByDescription = cInput._FindAxisByDescription(cInput._axisName[index]);
          cInput._individualAxisGrav[cInput._makeAxis[axisByDescription, 0]] = float.Parse(strArray20[index]);
          cInput._individualAxisGrav[cInput._makeAxis[axisByDescription, 1]] = float.Parse(strArray20[index]);
        }
      }
      if (PlayerPrefs.HasKey("cInput_indAxDead"))
      {
        string[] strArray21 = PlayerPrefs.GetString("cInput_indAxDead").Split('*');
        for (int index = 0; index < strArray21.Length - 1; ++index)
        {
          int axisByDescription = cInput._FindAxisByDescription(cInput._axisName[index]);
          cInput._individualAxisDead[cInput._makeAxis[axisByDescription, 0]] = float.Parse(strArray21[index]);
          cInput._individualAxisDead[cInput._makeAxis[axisByDescription, 1]] = float.Parse(strArray21[index]);
        }
      }
      if (PlayerPrefs.HasKey("cInput_saveCals"))
      {
        string[] strArray22 = PlayerPrefs.GetString("cInput_saveCals").Split('*');
        for (int index = 0; index < strArray22.Length - 1; ++index)
          cInput._axisType[index] = int.Parse(strArray22[index]);
      }
      if (PlayerPrefs.HasKey("cInput_calsVals"))
        cInput._CalibrationValuesFromString(PlayerPrefs.GetString("cInput_calsVals"));
      cInput._cKeysLoaded = true;
    }
  }

  private static void _LoadExternalInputs()
  {
    cInput._externalSaving = true;
    string[] strArray1 = cInput._exAxis.Split('¿');
    string[] strArray2 = cInput._exDefaults.Split('¿');
    string[] strArray3 = cInput._exInputs.Split('¿');
    cInput.allowDuplicates = cInput._exAllowDuplicates == "True";
    cInput._inputLength = int.Parse(strArray2[0]) - 1;
    string[] strArray4 = strArray2[1].Split('#');
    string[] strArray5 = strArray4[0].Split('*');
    string[] strArray6 = strArray4[1].Split('*');
    string[] strArray7 = strArray4[2].Split('*');
    string[] strArray8 = strArray4[3].Split('*');
    string[] strArray9 = strArray4[4].Split('*');
    for (int _num = 0; _num < strArray5.Length - 1; ++_num)
      cInput._SetDefaultKey(_num, strArray5[_num], strArray6[_num], strArray7[_num], strArray8[_num], strArray9[_num]);
    if (!string.IsNullOrEmpty(strArray3[0]))
    {
      string str1 = strArray3[0];
      string str2 = strArray3[1];
      string str3 = strArray3[2];
      string str4 = strArray3[3];
      string str5 = strArray3[4];
      string str6 = strArray3[5];
      string str7 = strArray3[6];
      string[] strArray10 = str1.Split('*');
      string[] strArray11 = str2.Split('*');
      string[] strArray12 = str3.Split('*');
      string[] strArray13 = str4.Split('*');
      string[] strArray14 = str5.Split('*');
      string[] strArray15 = str6.Split('*');
      string[] strArray16 = str7.Split('*');
      for (int index = 0; index < strArray10.Length - 1; ++index)
      {
        if (strArray10[index] == cInput._defaultStrings[index, 0])
        {
          cInput._inputName[index] = strArray10[index];
          cInput._inputPrimary[index] = cInput._ConvertString2Key(strArray11[index]);
          cInput._inputSecondary[index] = cInput._ConvertString2Key(strArray12[index]);
          cInput._axisPrimary[index] = strArray13[index];
          cInput._axisSecondary[index] = strArray14[index];
          cInput._modifierUsedPrimary[index] = cInput._ConvertString2Key(strArray15[index]);
          cInput._modifierUsedSecondary[index] = cInput._ConvertString2Key(strArray16[index]);
        }
      }
      for (int index1 = 0; index1 < strArray5.Length - 1; ++index1)
      {
        for (int index2 = 0; index2 < strArray10.Length - 1; ++index2)
        {
          if (strArray10[index2] == cInput._defaultStrings[index1, 0])
          {
            cInput._inputName[index1] = strArray10[index2];
            cInput._inputPrimary[index1] = cInput._ConvertString2Key(strArray11[index2]);
            cInput._inputSecondary[index1] = cInput._ConvertString2Key(strArray12[index2]);
            cInput._axisPrimary[index1] = strArray13[index2];
            cInput._axisSecondary[index1] = strArray14[index2];
            cInput._modifierUsedPrimary[index2] = cInput._ConvertString2Key(strArray15[index2]);
            cInput._modifierUsedSecondary[index2] = cInput._ConvertString2Key(strArray16[index2]);
          }
        }
      }
    }
    if (!string.IsNullOrEmpty(strArray1[0]))
    {
      string exAxisInverted = cInput._exAxisInverted;
      string[] strArray17;
      if (!string.IsNullOrEmpty(exAxisInverted))
        strArray17 = exAxisInverted.Split('*');
      else
        strArray17 = (string[]) null;
      string[] strArray18 = strArray17;
      string[] strArray19 = strArray1[0].Split('#');
      string[] strArray20 = strArray19[0].Split('*');
      string[] strArray21 = strArray19[1].Split('*');
      string[] strArray22 = strArray19[2].Split('*');
      int num = int.Parse(strArray19[3]);
      for (int _num = 0; _num < num; ++_num)
      {
        int _negative = int.Parse(strArray21[_num]);
        int _positive = int.Parse(strArray22[_num]);
        cInput._SetAxis(_num, strArray20[_num], _negative, _positive);
        if (!string.IsNullOrEmpty(exAxisInverted))
          cInput._invertAxis[_num] = strArray18[_num] == "True";
      }
    }
    if (strArray1.Length > 1 && !string.IsNullOrEmpty(strArray1[1]))
    {
      string[] strArray23 = strArray1[1].Split('*');
      for (int index = 0; index < strArray23.Length - 1; ++index)
      {
        int axisByDescription = cInput._FindAxisByDescription(cInput._axisName[index]);
        cInput._individualAxisSens[cInput._makeAxis[axisByDescription, 0]] = float.Parse(strArray23[index]);
        cInput._individualAxisSens[cInput._makeAxis[axisByDescription, 1]] = float.Parse(strArray23[index]);
      }
    }
    if (strArray1.Length > 2 && !string.IsNullOrEmpty(strArray1[2]))
    {
      string[] strArray24 = strArray1[2].Split('*');
      for (int index = 0; index < strArray24.Length - 1; ++index)
      {
        int axisByDescription = cInput._FindAxisByDescription(cInput._axisName[index]);
        cInput._individualAxisGrav[cInput._makeAxis[axisByDescription, 0]] = float.Parse(strArray24[index]);
        cInput._individualAxisGrav[cInput._makeAxis[axisByDescription, 1]] = float.Parse(strArray24[index]);
      }
    }
    if (strArray1.Length > 3 && !string.IsNullOrEmpty(strArray1[3]))
    {
      string[] strArray25 = strArray1[3].Split('*');
      for (int index = 0; index < strArray25.Length - 1; ++index)
      {
        int axisByDescription = cInput._FindAxisByDescription(cInput._axisName[index]);
        cInput._individualAxisDead[cInput._makeAxis[axisByDescription, 0]] = float.Parse(strArray25[index]);
        cInput._individualAxisDead[cInput._makeAxis[axisByDescription, 1]] = float.Parse(strArray25[index]);
      }
    }
    if (!string.IsNullOrEmpty(cInput._exCalibrations))
    {
      string[] strArray26 = cInput._exCalibrations.Split('*');
      for (int index = 1; index <= strArray26.Length - 2; ++index)
        cInput._axisType[index] = int.Parse(strArray26[index]);
    }
    if (!string.IsNullOrEmpty(cInput._exCalibrationValues))
      cInput._CalibrationValuesFromString(cInput._exCalibrationValues);
    cInput._cKeysLoaded = true;
  }

  public static void ResetInputs()
  {
    cInput._cInputInit();
    for (int _num = 0; _num < cInput._inputLength + 1; ++_num)
    {
      cInput._SetKey(_num, cInput._defaultStrings[_num, 0], cInput._defaultStrings[_num, 1], cInput._defaultStrings[_num, 2]);
      cInput._modifierUsedPrimary[_num] = cInput._ConvertString2Key(cInput._defaultStrings[_num, 3]);
      cInput._modifierUsedSecondary[_num] = cInput._ConvertString2Key(cInput._defaultStrings[_num, 4]);
      cInput._individualAxisSens[_num] = cInput._defaultAxes[_num, 0];
      cInput._individualAxisGrav[_num] = cInput._defaultAxes[_num, 1];
      cInput._individualAxisDead[_num] = cInput._defaultAxes[_num, 2];
    }
    for (int index = 0; index < cInput._axisLength; ++index)
      cInput._invertAxis[index] = false;
    cInput.Clear();
    cInput._SaveDefaults();
    cInput._SaveInputs();
    cInput._SaveAxInverted();
  }

  public static void Clear()
  {
    cInput._cInputInit();
    Logger.Warning("Clearing out all cInput related values from PlayerPrefs");
    PlayerPrefs.DeleteKey("cInput_axInv");
    PlayerPrefs.DeleteKey("cInput_axis");
    PlayerPrefs.DeleteKey("cInput_indAxSens");
    PlayerPrefs.DeleteKey("cInput_indAxGrav");
    PlayerPrefs.DeleteKey("cInput_indAxDead");
    PlayerPrefs.DeleteKey("cInput_count");
    PlayerPrefs.DeleteKey("cInput_defaults");
    PlayerPrefs.DeleteKey("cInput_descr");
    PlayerPrefs.DeleteKey("cInput_inp");
    PlayerPrefs.DeleteKey("cInput_alt_inp");
    PlayerPrefs.DeleteKey("cInput_inpStr");
    PlayerPrefs.DeleteKey("cInput_alt_inpStr");
    PlayerPrefs.DeleteKey("cInput_dubl");
    PlayerPrefs.DeleteKey("cInput_saveCals");
    PlayerPrefs.DeleteKey("cInput_calsVals");
    PlayerPrefs.DeleteKey("cInput_modifierStr");
    PlayerPrefs.DeleteKey("cInput_alt_modifierStr");
  }

  private static bool _AxisInverted(int hash, bool invertedStatus, string description = "")
  {
    cInput._cInputInit();
    int axisByHash = cInput._FindAxisByHash(hash);
    if (axisByHash > -1)
    {
      cInput._invertAxis[axisByHash] = invertedStatus;
      cInput._SaveAxInverted();
      return invertedStatus;
    }
    Logger.WarningFormat("Couldn't find an axis match for {0} while trying to set inversion status. Is it possible you typed it wrong?", (object) (string.IsNullOrEmpty(description) ? "axis with hashcode of " + (object) hash : description));
    return false;
  }

  public static bool AxisInverted(string description, bool invertedStatus)
  {
    return cInput._AxisInverted(description.GetHashCode(), invertedStatus, description);
  }

  public static bool AxisInverted(int descriptionHash, bool invertedStatus)
  {
    return cInput._AxisInverted(descriptionHash, invertedStatus, string.Empty);
  }

  private static bool _AxisInverted(int hash, string description = "")
  {
    cInput._cInputInit();
    int axisByHash = cInput._FindAxisByHash(hash);
    if (axisByHash > -1)
      return cInput._invertAxis[axisByHash];
    Logger.WarningFormat("Couldn't find an axis match for {0} while trying to get inversion status. Is it possible you typed it wrong?", (object) (string.IsNullOrEmpty(description) ? "axis with hashcode of " + (object) hash : description));
    return false;
  }

  public static bool AxisInverted(string description)
  {
    return cInput._AxisInverted(description.GetHashCode(), description);
  }

  public static bool AxisInverted(int descriptionHash)
  {
    return cInput._AxisInverted(descriptionHash, string.Empty);
  }

  public static bool ShowMenu()
  {
    cInput._cInputInit();
    Logger.Error("cInput.ShowMenu() has been deprecated. Please use the appropriate cGUI variable, such as cGUI.showingAnyGUI");
    return false;
  }

  public static void ShowMenu(bool state)
  {
    cInput.ShowMenu(state, cInput._menuHeaderString, cInput._menuActionsString, cInput._menuInputsString, cInput._menuButtonsString);
  }

  public static void ShowMenu(bool state, string menuHeader)
  {
    cInput.ShowMenu(state, menuHeader, cInput._menuActionsString, cInput._menuInputsString, cInput._menuButtonsString);
  }

  public static void ShowMenu(bool state, string menuHeader, string menuActions)
  {
    cInput.ShowMenu(state, menuHeader, menuActions, cInput._menuInputsString, cInput._menuButtonsString);
  }

  public static void ShowMenu(
    bool state,
    string menuHeader,
    string menuActions,
    string menuInputs)
  {
    cInput.ShowMenu(state, menuHeader, menuActions, menuInputs, cInput._menuButtonsString);
  }

  public static void ShowMenu(
    bool state,
    string menuHeader,
    string menuActions,
    string menuInputs,
    string menuButtons)
  {
    cInput._cInputInit();
    Logger.Error("cInput.ShowMenu() has been deprecated. Please use the appropriate cGUI function, such as cGUI.ToggleGUI()");
  }

  private static void _cInputInit()
  {
    if (cInput._hascObject)
      return;
    Logger.Error("cInput is being accessed without an instance. An instance is required to update this class.");
  }

  private void _CheckingDuplicates(int _num, int _count)
  {
    if (cInput.allowDuplicates)
      return;
    for (int index = 0; index < cInput.length; ++index)
    {
      if (_count == 1)
      {
        if (_num != index && cInput._inputPrimary[_num] == cInput._inputPrimary[index] && cInput._modifierUsedPrimary[_num] == cInput._modifierUsedPrimary[index])
          cInput._inputPrimary[index] = KeyCode.None;
        if (cInput._inputPrimary[_num] == cInput._inputSecondary[index] && cInput._modifierUsedPrimary[_num] == cInput._modifierUsedSecondary[index])
          cInput._inputSecondary[index] = KeyCode.None;
      }
      if (_count == 2)
      {
        if (cInput._inputSecondary[_num] == cInput._inputPrimary[index] && cInput._modifierUsedSecondary[_num] == cInput._modifierUsedPrimary[index])
          cInput._inputPrimary[index] = KeyCode.None;
        if (_num != index && cInput._inputSecondary[_num] == cInput._inputSecondary[index] && cInput._modifierUsedSecondary[_num] == cInput._modifierUsedSecondary[index])
          cInput._inputSecondary[index] = KeyCode.None;
      }
    }
  }

  private void _CheckingDuplicateStrings(int _num, int _count)
  {
    if (cInput.allowDuplicates)
      return;
    for (int index = 0; index < cInput.length; ++index)
    {
      if (_count == 1)
      {
        if (_num != index && cInput._axisPrimary[_num] == cInput._axisPrimary[index])
        {
          cInput._axisPrimary[index] = string.Empty;
          cInput._inputPrimary[index] = KeyCode.None;
        }
        if (cInput._axisPrimary[_num] == cInput._axisSecondary[index])
        {
          cInput._axisSecondary[index] = string.Empty;
          cInput._inputSecondary[index] = KeyCode.None;
        }
      }
      if (_count == 2)
      {
        if (cInput._axisSecondary[_num] == cInput._axisPrimary[index])
        {
          cInput._axisPrimary[index] = string.Empty;
          cInput._inputPrimary[index] = KeyCode.None;
        }
        if (_num != index && cInput._axisSecondary[_num] == cInput._axisSecondary[index])
        {
          cInput._axisSecondary[index] = string.Empty;
          cInput._inputSecondary[index] = KeyCode.None;
        }
      }
    }
  }

  private void _InputScans()
  {
    KeyCode keyCode1 = KeyCode.None;
    if (Input.GetKey(KeyCode.Escape))
    {
      if (cInput._cScanInput == 1)
      {
        cInput._inputPrimary[cInput._cScanIndex] = KeyCode.None;
        cInput._axisPrimary[cInput._cScanIndex] = string.Empty;
        cInput._cScanInput = 0;
      }
      if (cInput._cScanInput == 2)
      {
        cInput._inputSecondary[cInput._cScanIndex] = KeyCode.None;
        cInput._axisSecondary[cInput._cScanIndex] = string.Empty;
        cInput._cScanInput = 0;
      }
    }
    if (cInput._scanning && Input.anyKeyDown && !Input.GetKey(KeyCode.Escape))
    {
      KeyCode keyCode2 = KeyCode.None;
      for (int index1 = 0; index1 < 450; ++index1)
      {
        KeyCode key = (KeyCode) index1;
        if (key.ToString().StartsWith("Mouse"))
        {
          if (!cInput._allowMouseButtons)
            continue;
        }
        else if (key.ToString().StartsWith("Joystick"))
        {
          if (!cInput._allowJoystickButtons)
            continue;
        }
        else if (!cInput._allowKeyboard)
          continue;
        for (int index2 = 0; index2 < cInput._modifiers.Count; ++index2)
        {
          bool flag1 = false;
          for (int index3 = 0; index3 < cInput._modifiers.Count; ++index3)
          {
            if (Input.GetKey(cInput._modifiers[index3]) && index3 != index2)
            {
              flag1 = true;
              break;
            }
          }
          if (Input.GetKeyDown(cInput._modifiers[index2]))
          {
            if (!flag1)
              return;
          }
          else if (Input.GetKeyDown(key))
          {
            keyCode2 = key;
            keyCode1 = key;
            bool flag2 = false;
            for (int index4 = 0; index4 < cInput._markedAsAxis.Count; ++index4)
            {
              if (cInput._cScanIndex == cInput._markedAsAxis[index4])
              {
                flag2 = true;
                break;
              }
            }
            if (Input.GetKey(cInput._modifiers[index2]) && !flag2)
            {
              keyCode1 = cInput._modifiers[index2];
              break;
            }
          }
        }
      }
      if (keyCode2 != KeyCode.None)
      {
        bool flag = true;
        for (int index = 0; index < cInput._forbiddenKeys.Count; ++index)
        {
          if (keyCode2 == cInput._forbiddenKeys[index])
            flag = false;
        }
        if (flag)
        {
          if (cInput._cScanInput == 1)
          {
            cInput._inputPrimary[cInput._cScanIndex] = keyCode2;
            cInput._modifierUsedPrimary[cInput._cScanIndex] = keyCode1;
            cInput._axisPrimary[cInput._cScanIndex] = string.Empty;
            this._CheckingDuplicates(cInput._cScanIndex, cInput._cScanInput);
          }
          if (cInput._cScanInput == 2)
          {
            cInput._inputSecondary[cInput._cScanIndex] = keyCode2;
            cInput._modifierUsedSecondary[cInput._cScanIndex] = keyCode1;
            cInput._axisSecondary[cInput._cScanIndex] = string.Empty;
            this._CheckingDuplicates(cInput._cScanIndex, cInput._cScanInput);
          }
        }
        cInput._cScanInput = 0;
      }
    }
    if (cInput._allowMouseButtons)
    {
      for (int index = 0; index < cInput._modifiers.Count; ++index)
      {
        if (Input.GetKey(cInput._modifiers[index]) && !Input.GetKeyDown(cInput._modifiers[index]))
        {
          keyCode1 = cInput._modifiers[index];
          break;
        }
      }
      if ((double) Input.GetAxis("Mouse Wheel") > 0.0 && !Input.GetKey(KeyCode.Escape))
      {
        if (cInput._cScanInput == 1)
        {
          cInput._modifierUsedPrimary[cInput._cScanIndex] = keyCode1;
          cInput._axisPrimary[cInput._cScanIndex] = "Mouse Wheel Up";
        }
        if (cInput._cScanInput == 2)
        {
          cInput._modifierUsedSecondary[cInput._cScanIndex] = keyCode1;
          cInput._axisSecondary[cInput._cScanIndex] = "Mouse Wheel Up";
        }
        this._CheckingDuplicateStrings(cInput._cScanIndex, cInput._cScanInput);
        cInput._cScanInput = 0;
      }
      else if ((double) Input.GetAxis("Mouse Wheel") < 0.0 && !Input.GetKey(KeyCode.Escape))
      {
        if (cInput._cScanInput == 1)
        {
          cInput._modifierUsedPrimary[cInput._cScanIndex] = keyCode1;
          cInput._axisPrimary[cInput._cScanIndex] = "Mouse Wheel Down";
        }
        if (cInput._cScanInput == 2)
        {
          cInput._modifierUsedSecondary[cInput._cScanIndex] = keyCode1;
          cInput._axisSecondary[cInput._cScanIndex] = "Mouse Wheel Down";
        }
        this._CheckingDuplicateStrings(cInput._cScanIndex, cInput._cScanInput);
        cInput._cScanInput = 0;
      }
    }
    if (cInput._allowMouseAxis)
    {
      for (int index = 0; index < cInput._modifiers.Count; ++index)
      {
        if (Input.GetKey(cInput._modifiers[index]) && !Input.GetKeyDown(cInput._modifiers[index]))
        {
          keyCode1 = cInput._modifiers[index];
          break;
        }
      }
      if ((double) Input.GetAxis("Mouse Horizontal") < -(double) cInput.deadzone && !Input.GetKey(KeyCode.Escape))
      {
        if (cInput._cScanInput == 1)
        {
          cInput._modifierUsedPrimary[cInput._cScanIndex] = keyCode1;
          cInput._axisPrimary[cInput._cScanIndex] = "Mouse Left";
        }
        if (cInput._cScanInput == 2)
        {
          cInput._modifierUsedSecondary[cInput._cScanIndex] = keyCode1;
          cInput._axisSecondary[cInput._cScanIndex] = "Mouse Left";
        }
        this._CheckingDuplicateStrings(cInput._cScanIndex, cInput._cScanInput);
        cInput._cScanInput = 0;
      }
      else if ((double) Input.GetAxis("Mouse Horizontal") > (double) cInput.deadzone && !Input.GetKey(KeyCode.Escape))
      {
        if (cInput._cScanInput == 1)
        {
          cInput._modifierUsedPrimary[cInput._cScanIndex] = keyCode1;
          cInput._axisPrimary[cInput._cScanIndex] = "Mouse Right";
        }
        if (cInput._cScanInput == 2)
        {
          cInput._modifierUsedSecondary[cInput._cScanIndex] = keyCode1;
          cInput._axisSecondary[cInput._cScanIndex] = "Mouse Right";
        }
        this._CheckingDuplicateStrings(cInput._cScanIndex, cInput._cScanInput);
        cInput._cScanInput = 0;
      }
      if ((double) Input.GetAxis("Mouse Vertical") > (double) cInput.deadzone && !Input.GetKey(KeyCode.Escape))
      {
        if (cInput._cScanInput == 1)
        {
          cInput._modifierUsedPrimary[cInput._cScanIndex] = keyCode1;
          cInput._axisPrimary[cInput._cScanIndex] = "Mouse Up";
        }
        if (cInput._cScanInput == 2)
        {
          cInput._modifierUsedSecondary[cInput._cScanIndex] = keyCode1;
          cInput._axisSecondary[cInput._cScanIndex] = "Mouse Up";
        }
        this._CheckingDuplicateStrings(cInput._cScanIndex, cInput._cScanInput);
        cInput._cScanInput = 0;
      }
      else if ((double) Input.GetAxis("Mouse Vertical") < -(double) cInput.deadzone && !Input.GetKey(KeyCode.Escape))
      {
        if (cInput._cScanInput == 1)
        {
          cInput._modifierUsedPrimary[cInput._cScanIndex] = keyCode1;
          cInput._axisPrimary[cInput._cScanIndex] = "Mouse Down";
        }
        if (cInput._cScanInput == 2)
        {
          cInput._modifierUsedSecondary[cInput._cScanIndex] = keyCode1;
          cInput._axisSecondary[cInput._cScanIndex] = "Mouse Down";
        }
        this._CheckingDuplicateStrings(cInput._cScanIndex, cInput._cScanInput);
        cInput._cScanInput = 0;
      }
    }
    if (!cInput._allowJoystickAxis)
      return;
    float num = 0.25f;
    for (int index5 = 1; index5 <= cInput._numGamepads; ++index5)
    {
      for (int index6 = 1; index6 <= 10; ++index6)
      {
        string joyString = cInput._joyStrings[index5, index6];
        string description1 = cInput._joyStringsPos[index5, index6];
        string description2 = cInput._joyStringsNeg[index5, index6];
        float b = Input.GetAxisRaw(joyString);
        bool flag = false;
        if (cInput._axisRawValues.ContainsKey(joyString) && !Mathf.Approximately(cInput._axisRawValues[joyString], b))
          flag = true;
        if (flag)
        {
          if (Application.platform == RuntimePlatform.OSXEditor || Application.platform == RuntimePlatform.OSXPlayer || Application.platform == RuntimePlatform.OSXWebPlayer)
          {
            if (index6 == 5 || index6 == 6)
            {
              string joystickName = Input.GetJoystickNames()[index5 - 1];
              string[] strArray = new string[4]
              {
                string.Empty,
                "Microsoft Wireless 360 Controller",
                "Mad Catz, Inc. Mad Catz FPS Pro GamePad",
                "©Microsoft Corporation Controller"
              };
              foreach (string str in strArray)
              {
                if (joystickName == str)
                {
                  b = (float) (((double) b + 1.0) / 2.0);
                  break;
                }
              }
            }
          }
          else if (index6 == 3)
          {
            string joyStringsPo1 = cInput._joyStringsPos[index5, 9];
            string joyStringsPo2 = cInput._joyStringsPos[index5, 10];
            if ((double) cInput._GetCalibratedAxisInput(joyStringsPo1) > (double) num)
            {
              description1 = joyStringsPo1;
              description2 = joyStringsPo1;
            }
            else if ((double) cInput._GetCalibratedAxisInput(joyStringsPo2) > (double) num)
            {
              description1 = joyStringsPo2;
              description2 = joyStringsPo2;
            }
          }
          float f = (double) b >= 0.0 ? cInput._GetCalibratedAxisInput(description1) : cInput._GetCalibratedAxisInput(description2);
          if (cInput._scanning && (double) Mathf.Abs(f) > (double) num && !Input.GetKey(KeyCode.Escape))
          {
            for (int index7 = 0; index7 < cInput._modifiers.Count; ++index7)
            {
              if (Input.GetKey(cInput._modifiers[index7]) && !Input.GetKeyDown(cInput._modifiers[index7]))
              {
                keyCode1 = cInput._modifiers[index7];
                break;
              }
            }
            switch (cInput._cScanInput)
            {
              case 1:
                if ((double) f > (double) num)
                {
                  cInput._modifierUsedPrimary[cInput._cScanIndex] = keyCode1;
                  cInput._axisPrimary[cInput._cScanIndex] = description1;
                }
                else if ((double) f < -(double) num)
                {
                  cInput._modifierUsedPrimary[cInput._cScanIndex] = keyCode1;
                  cInput._axisPrimary[cInput._cScanIndex] = description2;
                }
                this._CheckingDuplicateStrings(cInput._cScanIndex, cInput._cScanInput);
                cInput._cScanInput = 0;
                return;
              case 2:
                if ((double) f > (double) num)
                {
                  cInput._modifierUsedSecondary[cInput._cScanIndex] = keyCode1;
                  cInput._axisSecondary[cInput._cScanIndex] = description1;
                }
                else if ((double) f < -(double) num)
                {
                  cInput._modifierUsedSecondary[cInput._cScanIndex] = keyCode1;
                  cInput._axisSecondary[cInput._cScanIndex] = description2;
                }
                this._CheckingDuplicateStrings(cInput._cScanIndex, cInput._cScanInput);
                cInput._cScanInput = 0;
                return;
              default:
                continue;
            }
          }
        }
      }
    }
  }
}
