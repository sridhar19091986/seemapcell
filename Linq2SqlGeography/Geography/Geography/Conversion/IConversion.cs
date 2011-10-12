///-----------------------------------
///作者: xgluxv@hotmail.com
///日期: 2010-8-13
///-----------------------------------

namespace Geography.CoordinateSystem
{
    /// <summary>
    /// 各种坐标系转换的接口
    /// </summary>
    /// <typeparam name="T"></typeparam>
    /// <typeparam name="TResult"></typeparam>
    public interface IConversion<T,TResult>
    {
        TResult Convert(T t);
    }
}
