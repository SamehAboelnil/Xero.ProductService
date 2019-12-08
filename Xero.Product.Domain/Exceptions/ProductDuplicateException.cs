using System;
using System.Runtime.Serialization;

namespace Xero.Product.Data
{
    [Serializable]
    public class ProductDuplicateException : Exception
    {
        public ProductDuplicateException()
        {
        }

        public ProductDuplicateException(string message) : base(message)
        {
        }

        public ProductDuplicateException(string message, Exception innerException) : base(message, innerException)
        {
        }

        protected ProductDuplicateException(SerializationInfo info, StreamingContext context) : base(info, context)
        {
        }
    }
}