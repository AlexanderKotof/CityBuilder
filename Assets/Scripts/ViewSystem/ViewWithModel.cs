namespace ViewSystem
{
    public abstract class ViewWithModel<TModel> : ViewBase
        where TModel : IViewModel
    {
        public TModel Model { get; private set; }

        public virtual void Initialize(TModel model)
        {
            Model = model;
        }
        
        public virtual void Deinit()
        {

        }
    }
}