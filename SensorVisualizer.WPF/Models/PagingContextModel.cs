namespace SensorVisualizer.WPF.Models
{
    public class PagingContextModel : PropertyChangedModel
    {
        private int _currentPage;
        private int _totalPages;
        public int CurrentPage
        {
            get => _currentPage;
            set
            {
                _currentPage = value;
                PropertiesChanged(nameof(CurrentPage), nameof(EnableNextButton), nameof(EnablePrevButton));
            }
        }
        public int TotalPages
        {
            get => _totalPages;
            set
            {
                _totalPages = value;
                _currentPage = _totalPages > 0 ? 1 : 0;
                PropertiesChanged(nameof(TotalPages), nameof(CurrentPage), nameof(ShowPaging), nameof(EnableNextButton), nameof(EnablePrevButton));
            }

        }
        public bool ShowPaging => _totalPages > 1;
        public bool EnableNextButton => _currentPage + 1 <= _totalPages;
        public bool EnablePrevButton => _currentPage > 1;
    }
}
