using Newtonsoft.Json.Linq;
using System;
using System.Collections.Generic;

/// <summary>
/// things like resource avatar, color...
/// can be define by outside user
/// </summary>

namespace GameCore.Base
{
    public class Resource
    {
        public enum Classify
        {
            Icon, Color, Model, Bone, Avatar, Face, Texture
            , Animate, Partical, Terrain, Text, Sound, Image
            , Other
        }
        public Classify Classification;
        public string Name = null;
        public int[] ParInt = null;
        public float[] ParFloat = null;
        public string[] ParStr = null;
        public bool[] ParBool = null;
        public void ClearPar()
        {
            ParInt = null;
            ParFloat = null;
            ParStr = null;
            ParBool = null;
        }
        public bool CopyFrom(Resource resource)
        {
            if (resource == null)
                return true;
            try
            {
                Name = resource.Name;
                if (resource.ParInt != null)
                {
                    ParInt = new int[resource.ParInt.Length];
                    Array.Copy(resource.ParInt, ParInt, resource.ParInt.Length);
                }
                if (resource.ParFloat != null)
                {
                    ParFloat = new float[resource.ParFloat.Length];
                    Array.Copy(resource.ParFloat, ParFloat, resource.ParFloat.Length);
                }
                if (resource.ParStr != null)
                {
                    ParStr = new string[resource.ParStr.Length];
                    Array.Copy(resource.ParStr, ParStr, resource.ParStr.Length);
                }
                if (resource.ParBool != null)
                {
                    ParBool = new bool[resource.ParBool.Length];
                    Array.Copy(resource.ParBool, ParBool, resource.ParBool.Length);
                }
            }
            catch (Exception) { return true; }
            return false;
        }
    }
    // down below are examples, need redefine by outside real using.
    public class ResourceColor : Resource
    {
        public enum OptCover
        {
            _start,
            sat, val, hue
                , _end
        }
        public enum OptOffset
        {
            _start = OptCover._end,
            sat, val, hue
                , _end
        }
        private enum OptBlendColorDegree
        {
            _start = OptOffset._end,
            second, third, fourth, fifth
                , _end
        }
        private enum _IntEnd { _end = OptBlendColorDegree._end}
        public enum OptAppendColor
        {
            _start,
            second, third, fourth, fifth
                , _end
        }
        public enum OptBlendColor
        {
            _start = OptAppendColor._end,
            second, third, fourth, fifth
                , _end
        }
        private enum _StrEnd { _end = OptBlendColor._end}
        public ResourceColor()
        {
            Init();
        }
        public void Init()
        {
            Classification = Classify.Color;
            ParInt = new int[(int)_IntEnd._end];
            ParStr = new string[(int)_StrEnd._end];
        }
        public bool IsLegal()
        {
            if (ParInt == null)
                return false;
            if (ParInt.Length < (int)_IntEnd._end)
                return false;
            if (ParStr == null)
                return false;
            if (ParStr.Length < (int)_StrEnd._end)
                return false;
            return true;
        }
        public bool SetOptionCover(OptCover option_type, int value)
        {
            if (!IsLegal())
                return true;
            try
            {
                ParInt[(int)option_type] = value;
            }
            catch (Exception) { return true; }
            return false;
        }
        public int GetOptionCover(OptCover option_type) => ParInt[(int)option_type];
        public bool SetOptionOffset(OptOffset option_type, int value)
        {
            if (!IsLegal())
                return true;
            try
            {
                ParInt[(int)option_type] = value;
            }
            catch (Exception) { return true; }
            return false;
        }
        public int GetOptionOffset(OptOffset option_type) => ParInt[(int)option_type];
        public bool SetAppendColor(OptAppendColor append_order, string color)
        {
            if (!IsLegal())
                return true;
            try
            {
                ParStr[(int)append_order] = color;
            }
            catch (Exception) { return true; }
            return false;
        }
        public string GetAppendColor(OptAppendColor append_order) => ParStr[(int)append_order];
        public bool SetBlendColor(OptBlendColor blend_order
            , string color, int degree)
        {
            if (!IsLegal())
                return true;
            try
            {
                ParStr[(int)blend_order] = color;
                ParInt[(int)blend_order - (int)OptBlendColor._start + (int)OptBlendColorDegree._start] = degree;
            }
            catch (Exception) { return true; }
            return false;
        }
        public KeyValuePair<string, int> GetBlendColor(OptBlendColor blend_order)
        {
            return new KeyValuePair<string, int>(
                ParStr[(int)blend_order],
                ParInt[(int)blend_order - (int)OptBlendColor._start + (int)OptBlendColorDegree._start]
                );
        }
    }
    public class ResourceAvatar : Resource
    {
        public enum Opt
        {
            _start,
            height, old, fat, strong,
            bone_size
                , _end
        }
        public enum OptFemale
        {
            _start = Opt._end,
            breast, waist, butt, beauty
                , _end
        }
        public enum OptMale
        {
            _start = OptFemale._end,
            bird_size, shoulder, waist
                , _end
        }
        public enum _IntEnd { _end = OptMale._end }
        public enum OptSex
        {
            _start,
            is_male
                , _end
        }
        public enum _BoolEnd { _end = OptSex._end}
        public ResourceAvatar()
        {
            Init();
        }
        public void Init()
        {
            Classification = Classify.Avatar;
            ParInt = new int[(int)_IntEnd._end];
            ParBool = new bool[(int)_BoolEnd._end];
        }
        public bool IsLegal()
        {
            if (ParInt == null)
                return false;
            if (ParInt.Length < (int)_IntEnd._end)
                return false;
            if (ParBool == null)
                return false;
            if (ParBool.Length < (int)_BoolEnd._end)
                return false;
            return true;
        }
        public bool SetOpt(Opt option_type, int value)
        {
            if (!IsLegal())
                return true;
            try
            {
                ParInt[(int)option_type] = value;
            }
            catch (Exception) { return true; }
            return false;
        }
        public int GetOpt(Opt type) => ParInt[(int)type];
        public bool SetOptFemale(OptFemale option_type, int value)
        {
            if (!IsLegal())
                return true;
            try
            {
                ParInt[(int)option_type] = value;
            }
            catch (Exception) { return true; }
            return false;
        }
        public int GetOptFemale(OptFemale type) => ParInt[(int)type];
        public bool SetOptMale(OptMale option_type, int value)
        {
            if (!IsLegal())
                return true;
            try
            {
                ParInt[(int)option_type] = value;
            }
            catch (Exception) { return true; }
            return false;
        }
        public int GetOptMale(OptMale type) => ParInt[(int)type];
        public bool SetSex(bool is_male = false)
        {
            if (!IsLegal())
                return true;
            try
            {
                ParBool[(int)OptSex.is_male] = is_male;
            }
            catch (Exception) { return true; }
            return false;
        }
        public bool GetIsMale() => ParBool[(int)OptSex.is_male];
    }
    public class ResourceFace : Resource
    {
        public enum Opt
        {
            _start,
            main, beauty, eyebrow, nose, mouse,
            ears, head
                , _end
        }
        public enum OptHair
        {
            _start = Opt._end,
            main, second, front, back
                , _end
        }
        private enum OptHairLength
        {
            _start = OptHair._end,
            main, second, front, back
                , _end
        }
        public enum OptEye
        {
            _start = OptHairLength._end,
            eyeL, ballL, eyeR, ballR
                , _end
        }
        public enum _IntEnd { _end = OptEye._end }
        public enum OptColor
        {
            _start,
            eyeL, eyeR, hair, hair_two, hair_three
                , _end
        }
        private enum OptFromTemplate
        {
            _start = OptColor._end,
            target
                , _end
        }
        public enum _StrEnd { _end = OptFromTemplate._end }
        public enum OptSize
        {
            _start,
            eyes, head,
            eyes_width, eyes_height, head_width, head_height
                , _end
        }
        public enum _FloatEnd { _end = OptSize._end }
        public enum OptBool
        {
            _start,
            is_male,
            is_from_template,
            eye_color_from_other_resource,
            hair_color_from_other_resource
                , _end
        }
        public enum _BoolEnd { _end = OptBool._end }
        public ResourceFace()
        {
            Init();
        }
        public void Init()
        {
            Classification = Classify.Avatar;
            ParInt = new int[(int)_IntEnd._end];
            ParStr = new string[(int)_StrEnd._end];
            ParFloat = new float[(int)_FloatEnd._end];
            ParBool = new bool[(int)_BoolEnd._end];
        }
        public bool IsLegal()
        {
            if (ParInt == null)
                return false;
            if (ParInt.Length < (int)_IntEnd._end)
                return false;
            if (ParStr == null)
                return false;
            if (ParStr.Length < (int)_StrEnd._end)
                return false;
            if (ParFloat == null)
                return false;
            if (ParFloat.Length < (int)_FloatEnd._end)
                return false;
            if (ParBool == null)
                return false;
            if (ParBool.Length < (int)_BoolEnd._end)
                return false;
            return true;
        }
        public bool SetOpt(Opt type, int value)
        {
            if (!IsLegal())
                return true;
            try
            {
                ParInt[(int)type] = value;
            }
            catch (Exception) { return true; }
            return false;
        }
        public int GetOpt(Opt type) => ParInt[(int)type];
        public bool SetSex(bool is_male = false)
        {
            if (!IsLegal())
                return true;
            try
            {
                ParBool[(int)OptBool.is_male] = is_male;
            }
            catch (Exception) { return true; }
            return false;
        }
        public bool GetIsMale() => ParBool[(int)OptBool.is_male];
        public bool SetOptHair(OptHair type, int value, int length)
        {
            if (!IsLegal())
                return true;
            try
            {
                ParInt[(int)type] = value;
                ParInt[(int)type - (int)OptHair._start + (int)OptHairLength._start] = length;
            }
            catch (Exception) { return true; }
            return false;
        }
        public int GetOptHair(OptHair type) => ParInt[(int)type];
        public int GetOptHairLength(OptHair type) => ParInt[(int)type - (int)OptHair._start + (int)OptHairLength._start];
        public bool SetOptEye(OptEye type, int value)
        {
            if (!IsLegal())
                return true;
            try
            {
                ParInt[(int)type] = value;
            }
            catch (Exception) { return true; }
            return false;
        }
        public int GetOptEye(OptEye type) => ParInt[(int)type];
        public bool SetSize(OptSize type, float scale)
        {
            if (!IsLegal())
                return true;
            try
            {
                ParFloat[(int)type] = scale;
            }
            catch (Exception) { return true; }
            return false;
        }
        public float GetSize(OptSize type) => ParFloat[(int)type];
        public bool SetColor(OptColor type, string color)
        {
            if (!IsLegal())
                return true;
            try
            {
                ParStr[(int)type] = color;
            }
            catch (Exception) { return true; }
            return false;
        }
        public string GetColor(OptColor type) => ParStr[(int)type];
        public bool SetTemplate(bool is_male, string template_name)
        {
            if (!IsLegal())
                return true;
            try
            {
                if (template_name == null)
                    return true;
                ParStr[(int)OptFromTemplate.target] = template_name;
                ParBool[(int)OptBool.is_from_template] = true;
                ParBool[(int)OptBool.is_male] = is_male;
            }
            catch (Exception) { return true; }
            return false;
        }
        public string GetTemplate() => ParStr[(int)OptFromTemplate.target];
    }
    public class ResourceIcon : Resource
    {
        public enum OptInt{
            MAIN,
            CENTRAL, BACKGROUND,
            CENTRAL_ROTATE,
            CENTRAL_OFFSET_X, CENTRAL_OFFSET_Y
        }
        public enum OptFloat{
            CENTRAL_SCALE
        }
        public enum OptStr{
            CENTRAL_COLOR, BACKGROUND_COLOR
        }
        public enum OptBool{
            CENTRAL_COLOR_FROM_OTHER, BACKGROUND_COLOR_FROM_OTHER
        }
        public ResourceIcon()
        {
            Init();
        }
        public void Init()
        {
            Classification = Classify.Avatar;
            ParInt = new int[Enum.GetValues(typeof(OptInt)).Length];
            ParStr = new string[Enum.GetValues(typeof(OptStr)).Length];
            ParFloat = new float[Enum.GetValues(typeof(OptFloat)).Length];
            ParBool = new bool[Enum.GetValues(typeof(OptBool)).Length];
        }
        public bool IsLegal()
        {
            if (ParInt == null)
                return false;
            if (ParInt.Length < Enum.GetValues(typeof(OptInt)).Length)
                return false;
            if (ParStr == null)
                return false;
            if (ParStr.Length < Enum.GetValues(typeof(OptStr)).Length)
                return false;
            if (ParFloat == null)
                return false;
            if (ParFloat.Length < Enum.GetValues(typeof(OptFloat)).Length)
                return false;
            if (ParBool == null)
                return false;
            if (ParBool.Length < Enum.GetValues(typeof(OptBool)).Length)
                return false;
            return true;
        }
        public bool SetInt(OptInt type, int value)
        {
            if (!IsLegal())
                return true;
            try
            {
                ParInt[(int)type] = value;
            }
            catch (Exception) { return true; }
            return false;
        }
        public int GetInt(OptInt type) => ParInt[(int)type];
        public bool SetStr(OptStr type, string value)
        {
            if (!IsLegal())
                return true;
            try
            {
                ParStr[(int)type] = value;
            }
            catch (Exception) { return true; }
            return false;
        }
        public string GetStr(OptStr type) => ParStr[(int)type];
        public bool SetBool(OptBool type, bool value)
        {
            if (!IsLegal())
                return true;
            try
            {
                ParBool[(int)type] = value;
            }
            catch (Exception) { return true; }
            return false;
        }
        public bool GetBool(OptBool type) => ParBool[(int)type];
        public bool SetFloat(OptFloat type, float value)
        {
            if (!IsLegal())
                return true;
            try
            {
                ParFloat[(int)type] = value;
            }
            catch (Exception) { return true; }
            return false;
        }
        public float GetFloat(OptFloat type) => ParFloat[(int)type];
    }
    /*
    public class ResourceAvatar : Resource
    {
        public enum Opt{
            _start,
                , _end
        }
        public enum _IntEnd { _end = Opt._end }
        public enum _StrEnd { _end }
        public enum _FloatEnd { _end }
        public enum _BoolEnd { _end }
        public ResourceAvatar()
        {
            Init();
        }
        public void Init()
        {
            Classification = Classify.Avatar;
            ParInt = new int[(int)_IntEnd._end];
            ParStr = new string[(int)_StrEnd._end];
            ParFloat = new float[(int)_FloatEnd._end];
            ParBool = new bool[(int)_BoolEnd._end];
        }
        public bool IsLegal()
        {
            if (ParInt == null)
                return false;
            if (ParInt.Length < (int)_IntEnd._end)
                return false;
            if (ParStr == null)
                return false;
            if (ParStr.Length < (int)_StrEnd._end)
                return false;
            if (ParFloat == null)
                return false;
            if (ParFloat.Length < (int)_FloatEnd._end)
                return false;
            if (ParBool == null)
                return false;
            if (ParBool.Length < (int)_BoolEnd._end)
                return false;
            return true;
        }
        public bool SetOpt(Opt type, int value)
        {
            if (!IsLegal())
                return true;
            try
            {
                ParInt[(int)type] = value;
            }
            catch (Exception) { return true; }
            return false;
        }
        public int GetOpt(Option type) => ParInt[(int)type];
    }
    */
}