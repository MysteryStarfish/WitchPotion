using Map;
using MessagePipe;

namespace UpdateButtonViewRequest
{
    public class UpdateButtonViewRequest
    {
    };

    public class UpdateButtonViewPublisher
    {
        private readonly IPublisher<UpdateButtonViewRequest> _publisher;

        public UpdateButtonViewPublisher(IPublisher<UpdateButtonViewRequest> publisher)
        {
            _publisher = publisher;
        }

        public void RequestUpdateButtonView()
        {
            _publisher.Publish(new UpdateButtonViewRequest());
        }
    }

    public class UpdateButtonViewSubscriber
    {
        private readonly ISubscriber<UpdateButtonViewRequest> _subscriber;
        private readonly MapViewer _view;

        public UpdateButtonViewSubscriber(ISubscriber<UpdateButtonViewRequest> subscriber, MapViewer view)
        {
            _subscriber = subscriber;
            _view = view;
            // Debug.Log("SceneChangeSubscriber subscribed successfully.");
            _subscriber.Subscribe(OnUpdateButtonViewRequested);
        }

        private void OnUpdateButtonViewRequested(UpdateButtonViewRequest request)
        {
            // Debug.Log($"Changing scene to: {request.ChosenIndex}");
            _view.UpdateText();
        }
    }
}

