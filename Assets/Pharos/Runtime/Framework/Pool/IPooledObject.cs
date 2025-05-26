namespace Pharos.Framework.Pool
{
    public interface IPooledObject
    {
        /// <summary>
        /// 出池前处理方法
        /// </summary>
        void OnPreprocessGet();
        
        /// <summary>
        /// 返回到对象池之前处理方法
        /// </summary>
        void OnPreprocessReturn();
    }
}