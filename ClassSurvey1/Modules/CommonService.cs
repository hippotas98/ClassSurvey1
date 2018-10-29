using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace ClassSurvey1.Modules
{
    public interface ITransientService
    {
    }
    public interface IScopedService
    {
    }

    public interface IValidator<T>
    {
        bool ValidateCreate(T entity);
        bool ValidateUpdate(T entity);
        bool ValidateDelete(T entity);
    }

    public class CommonService
    {
        protected IUnitOfWork UnitOfWork;

        public CommonService()
        {
        }

        public CommonService(IUnitOfWork UnitOfWork)
        {
            this.UnitOfWork = UnitOfWork;
        }
    }

    public enum ERRORCODE
    {
        NONE = 0,
        ITEM_NOT_EXISTED = 1,
        ITEM_NOT_UPLOAD = 2,
        ITEM_IS_DEFAULT = 3,
        ITEM_EXSITED = 4,
    }
}
