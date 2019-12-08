using System;
using System.Runtime.Serialization;

namespace Xero.Product.Data
{
    [Serializable]
    public class ProductOptionDuplicateException : Exception
    {
        public ProductOptionDuplicateException()
        {
        }

        public ProductOptionDuplicateException(string message) : base(message)
        {
        }

        public ProductOptionDuplicateException(string message, Exception innerException) : base(message, innerException)
        {
        }

        protected ProductOptionDuplicateException(SerializationInfo info, StreamingContext context) : base(info, context)
        {
        }
    }
}