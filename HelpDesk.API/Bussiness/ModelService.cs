using HelpDesk.API.DataAccess;
using HelpDesk.API.DTO_s;
using HelpDesk.API.GenericHelpers;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;

namespace HelpDesk.API.Bussiness
{
    public class ModelService: IModelService
    {
        private readonly IModel model;
        public ModelService(Model _model)
        {
            model = _model;
        }

        #region Product CURD Operation Interface implementation
        public IEnumerable<ModelDTO> GetModelList(ModelDTO obj)
        {
            var data = model.GetModelList(obj);
            var list = CustomDataReaderToGenericExtension.GetDataObjects<ModelDTO>(data);
            data.Close();
            return list;
        }
        public IEnumerable<ModelDTO> GetProductList(ModelDTO obj)
        {
            var data = model.GetProductList(obj);
            var list = CustomDataReaderToGenericExtension.GetDataObjects<ModelDTO>(data);
            data.Close();
            return list;
        }
        public ModelDTO InsertUpdateModel(ModelDTO obj)
        {
            try
            {
                var data = model.InsertUpdateModel(obj);
                if (data.HasRows)
                {
                    while (data.Read())
                    {
                        obj.message = data["message"].ToString();
                    }
                }
                else
                    obj.message = "0";
            }
            catch (Exception ex)
            {
                obj.message = ex.ToString();
                throw;
            }
            return obj;
        }
        public ModelDTO GetModelDetailsById(ModelDTO obj)
        {
            try
            {
                var data = model.GetModelDetailsById(obj);
                if (data.HasRows)
                {
                    while (data.Read())
                    {
                        obj.ProductId = int.Parse(data["ProductId"].ToString());
                        obj.ModelId = int.Parse(data["ModelId"].ToString());
                        obj.ModelName = data["ModelName"].ToString();
                        obj.isActive = bool.Parse(data["isActive"].ToString());
                    }
                }
            }
            catch (Exception ex)
            {
                obj.message = ex.ToString();
                throw;
            }
            return obj;
        }
        #endregion
    }
    public interface IModelService
    {
        #region Model CURD Operation Interface (declartion)
        IEnumerable<ModelDTO> GetModelList(ModelDTO obj);
        IEnumerable<ModelDTO> GetProductList(ModelDTO obj);
        ModelDTO InsertUpdateModel(ModelDTO obj);
        ModelDTO GetModelDetailsById(ModelDTO obj);
        #endregion
    }
}