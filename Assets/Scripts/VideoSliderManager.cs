using UnityEngine;
using UnityEngine.UI;
using UnityEngine.EventSystems;
using RenderHeads.Media.AVProVideo;

public class VideoSliderManager : MonoBehaviour
{
	public MediaPlayer _mediaPlayer;
	public Slider _sliderTime;
	private bool _isHoveringOverTimeline, _wasPlayingBeforeTimelineDrag, check;
	private TimeRange timelineRange;
	public ExperienceManager manager;
	public ToggleButton toggleButton;
	private void Start()
    {
		CreateTimelineDragEvents();
		_mediaPlayer = SceneManager.Instance.myMediaPlayer;
	}
    private void OnEnable()
    {
		check = true;
    }
    public void Offslider()
    {
		_sliderTime.interactable = false;
    }
    private void Update()
    {

		if (_mediaPlayer.Info != null)
		{
			timelineRange = GetTimelineRange();

		}

		if (_sliderTime)
		{
			double t = 0.0;
			if (timelineRange.duration > 0.0)
			{
				t = ((_mediaPlayer.Control.GetCurrentTime() - timelineRange.startTime) / timelineRange.duration);
			}
			_sliderTime.value = Mathf.Clamp01((float)t);
		}
        if (_sliderTime.value >=1)
        {
            if (check)
            {
				manager.Exit();
				check = false;
			}
        }
	}
    private void CreateTimelineDragEvents()
	{
		EventTrigger trigger = _sliderTime.gameObject.GetComponent<EventTrigger>();
		if (trigger != null)
		{
			EventTrigger.Entry entry = new EventTrigger.Entry();
			entry.eventID = EventTriggerType.PointerDown;
			entry.callback.AddListener((data) => { OnTimeSliderBeginDrag(); });
			trigger.triggers.Add(entry);

			entry = new EventTrigger.Entry();
			entry.eventID = EventTriggerType.Drag;
			entry.callback.AddListener((data) => { OnTimeSliderDrag(); });
			trigger.triggers.Add(entry);

			entry = new EventTrigger.Entry();
			entry.eventID = EventTriggerType.PointerUp;
			entry.callback.AddListener((data) => { OnTimeSliderEndDrag(); });
			trigger.triggers.Add(entry);

			entry = new EventTrigger.Entry();
			entry.eventID = EventTriggerType.PointerEnter;
			entry.callback.AddListener((data) => { OnTimelineBeginHover((PointerEventData)data); });
			trigger.triggers.Add(entry);

			entry = new EventTrigger.Entry();
			entry.eventID = EventTriggerType.PointerExit;
			entry.callback.AddListener((data) => { OnTimelineEndHover((PointerEventData)data); });
			trigger.triggers.Add(entry);
		}
	}
	private void OnTimelineBeginHover(PointerEventData eventData)
	{
		if (eventData.pointerCurrentRaycast.gameObject != null)
		{
			_isHoveringOverTimeline = true;
			//_sliderTime.transform.localScale = new Vector3(1f, 2.5f, 1f);
		}
	}

	private void OnTimelineEndHover(PointerEventData eventData)
	{
		_isHoveringOverTimeline = false;
		//_sliderTime.transform.localScale = new Vector3(1f, 1f, 1f);
	}
	private void OnTimeSliderBeginDrag()
	{
		if (_mediaPlayer && _mediaPlayer.Control != null)
		{
			_wasPlayingBeforeTimelineDrag = _mediaPlayer.Control.IsPlaying();
			//manager.VideoForwardStart();
			if (_wasPlayingBeforeTimelineDrag)
			{
				_mediaPlayer.Pause();
			}
			OnTimeSliderDrag();
		}
	}

	private void OnTimeSliderDrag()
	{
		if (_mediaPlayer && _mediaPlayer.Control != null)
		{
			TimeRange timelineRange = GetTimelineRange();
			double time = timelineRange.startTime + (_sliderTime.value * timelineRange.duration);
			_mediaPlayer.Control.Seek(time);
			_isHoveringOverTimeline = true;
			//newly added line
			_mediaPlayer.Play();
			//manager.VideoForwardStop();
			toggleButton.ToggleButton1();
			Debug.Log("Value changing");
		}
	}
	private void OnTimeSliderEndDrag()
	{
		if (_mediaPlayer && _mediaPlayer.Control != null)
		{
			
			if (_wasPlayingBeforeTimelineDrag)
			{
				_mediaPlayer.Play();
				_wasPlayingBeforeTimelineDrag = false;
			}
		}
	}
	private TimeRange GetTimelineRange()
	{
		if (_mediaPlayer.Info != null)
		{
			return Helper.GetTimelineRange(_mediaPlayer.Info.GetDuration(), _mediaPlayer.Control.GetSeekableTimes());
		}
		return new TimeRange();
	}

}
