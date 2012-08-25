using RecognitionLib;
using System;
using System.Collections.Generic;
using System.IO;
using System.Linq;
using Windows.Foundation;
using Windows.Foundation.Collections;
using Windows.UI;
using Windows.UI.Xaml;
using Windows.UI.Xaml.Controls;
using Windows.UI.Xaml.Controls.Primitives;
using Windows.UI.Xaml.Data;
using Windows.UI.Xaml.Input;
using Windows.UI.Xaml.Media;
using Windows.UI.Xaml.Navigation;
using Windows.UI.Xaml.Shapes;
using Windows.UI.Xaml.Media.Imaging;
using RecognitionLibrary;
using System.Collections.ObjectModel;
using MathNumericsStripped;
using System.Xml.Serialization;
using Windows.Storage.Pickers;
using System.Text;
using Windows.Storage;

// The Blank Page item template is documented at http://go.microsoft.com/fwlink/?LinkId=234238

namespace SchoolOfMagic
{
    /// <summary>
    /// An empty page that can be used on its own or navigated to within a Frame.
    /// </summary>
    public sealed partial class MainPage : Page
    {
        private bool isDrawing;
        private bool isTraining = false;
        private Color backgroundColor = Color.FromArgb(255, 0, 0, 0);
        private List<Rectangle> drawnRectangles = new List<Rectangle>();
        private Image star;
        private Image star_red;
        private int trainCounter = 3;
        private List<MatrixInputData> currentTrainingData = new List<MatrixInputData>();
        private List<Point> currentTrainingPoints = new List<Point>();
        private ObservableCollection<TrainSet> trained_spells = new ObservableCollection<TrainSet>();
        private Recognition recognition;

        private const double MIN_DIST = 5.0;
        private const int MAX_SPELLS = 15;
        private const int INPUT_WIDTH = 50;
        private const int INPUT_HEIGHT = 50;
        private const int START_TRAIN_COUNTER = 5;

        public MainPage()
        {
            star = new Image();
            star.Source = new BitmapImage(new Uri("ms-appx:///img/star.png"));
            star.Height = 25;
            star.Width = 25;

            star_red = new Image();
            star_red.Source = new BitmapImage(new Uri("ms-appx:///img/star_red.png"));
            star_red.Height = 25;
            star_red.Width = 25;

            this.InitializeComponent();
            this.DataContext = trained_spells;
        }

        /// <summary>
        /// Invoked when this page is about to be displayed in a Frame.
        /// </summary>
        /// <param name="e">Event data that describes how this page was reached.  The Parameter
        /// property is typically used to configure the page.</param>
        protected override void OnNavigatedTo(NavigationEventArgs e)
        {
        }

        private Point lastPoint = new Point(0,0);

        private void Grid_PointerMoved_1(object sender, PointerRoutedEventArgs e)
        {
            if (isDrawing)
            {
                var p = e.GetCurrentPoint(canvas).Position;

                if (dist(p, lastPoint) > MIN_DIST)
                {
                    Point newP = new Point(p.X * INPUT_WIDTH / canvas.ActualWidth, p.Y * INPUT_HEIGHT / canvas.ActualHeight);
                    currentTrainingPoints.Add(newP);
                    DrawCurve(p, lastPoint);
                    lastPoint = p;
                }
            }
        }

        private void DrawRect(int x, int y, bool yellow_star = true)
        {
            Rectangle newStar = new Rectangle();
            newStar.Height = star.Height;
            newStar.Width = star.Width;
            // Create an ImageBrush
            ImageBrush imgBrush = new ImageBrush();
            if(yellow_star)
                imgBrush.ImageSource = star.Source;
            else
                imgBrush.ImageSource = star_red.Source;

            // Fill rectangle with an ImageBrush
            newStar.Fill = imgBrush;
            newStar.Margin = new Thickness(x-star.Width/2, y-star.Height/2, 0, 0);
            canvas.Children.Add(newStar);
            if (yellow_star)
                drawnRectangles.Add(newStar);
        }

        private void DrawCurve(Point p1, Point p2)
        {
            int N = (int)(dist(p1,p2)/MIN_DIST); // maximum interpolation points

            for (int k = 0; k <= N; k++)
            {
                double f = (double)k/N;

                Point p = new Point(p1.X + f * (p2.X - p1.X), p1.Y + f * (p2.Y - p1.Y));
                DrawRect((int)p.X, (int)p.Y);
            }
        }

        private void DrawVec(MathNumericsStripped.Matrix vec)
        {
            for (int i = 0; i < vec.Height; i++ )
            {
                if (vec[i, 0] > 0)
                    DrawRect((int)((i % INPUT_WIDTH) / (double)INPUT_WIDTH * canvas.Width), (int)((i / INPUT_HEIGHT) / (double)INPUT_HEIGHT * canvas.Height), false);
            }
        }

        private double dist(Point point1, Point point2)
        {
            return Math.Sqrt((double)(point1.X - point2.X) * (point1.X - point2.X) + (double)(point1.Y - point2.Y) * (point1.Y - point2.Y));
        }

        private void Grid_PointerPressed_1(object sender, PointerRoutedEventArgs e)
        {
            // TODO: check if pointer was pressed for a longer time
            isDrawing = true;
            lastPoint = e.GetCurrentPoint(canvas).Position;
            currentTrainingPoints.Clear();
        }

        private void removeRects()
        {
            foreach (var rect in drawnRectangles)
            {
                canvas.Children.Remove(rect);
            }
        }

        private void Grid_PointerReleased_1(object sender, PointerRoutedEventArgs e)
        {
            if (isTraining)
            {
                trainCounter--;
                MatrixInputData aid = new MatrixInputData(newSpellNameTextBox.Text, DataManipulation.List2Pattern(INPUT_WIDTH, INPUT_HEIGHT, currentTrainingPoints));
                
                currentTrainingData.Add(aid);
                currentTrainingPoints.Clear();

                // no more training needed, create spell
                if (trainCounter == 0)
                {
                    isTraining = false;
                    //DrawVec(aid.Value);
                    trained_spells.Add(new TrainSet(newSpellNameTextBox.Text, newSpellDescriptionText.Text, currentTrainingData));
                    todoTextBlock.Text = "Congrats. You created a new spell.";
                }
                else
                {
                    todoTextBlock.Text = "train your spell. Trainings left: " + trainCounter;
                }
            }
            else if(recognition != null)
            {
                // convert pattern to matrix
                MathNumericsStripped.Matrix current_spelldata = DataManipulation.List2Pattern(INPUT_WIDTH, INPUT_HEIGHT, currentTrainingPoints);
                //DrawVec(current_spelldata);
                // try recognition by feed forwarding it into the net
                MathNumericsStripped.Matrix res = recognition.recognise(current_spelldata);
                // get the spell name back and display it
                bool displayed = false;
                for (int i = 0; i < res.Height; i++)
                {
                    if (res[i, 0] > 0.6)
                    {
                        todoTextBlock.Text = "You cast the spell named " + trained_spells[i].Name + ".";
                        displayed = true;
                        break;
                    }
                }
                if (!displayed)
                {
                    todoTextBlock.Text = "No spell was recognized.";
                }
                newSpellButton.IsEnabled = true;
            }
            removeRects();
            isDrawing = false;
        }

        /// <summary>
        /// Helper function, serializes an object into an xml string.
        /// credit: http://social.msdn.microsoft.com/Forums/en-US/csharplanguage/thread/5d08bc28-5b61-4c5a-8c4b-4665b1c929ea/
        /// </summary>
        /// <param name="obj"></param>
        /// <returns></returns>
        public static string SerializeToString(object obj)
        {
            XmlSerializer serializer = new XmlSerializer(obj.GetType());

            using (StringWriter writer = new StringWriter())
            {
                serializer.Serialize(writer, obj);

                return writer.ToString();
            }
        }

        /// <summary>
        /// Get a serialized object back.
        /// credit: http://social.msdn.microsoft.com/Forums/en-US/csharplanguage/thread/5d08bc28-5b61-4c5a-8c4b-4665b1c929ea/
        /// </summary>
        /// <typeparam name="T"></typeparam>
        /// <param name="xml"></param>
        /// <returns></returns>
        public static T SerializeFromString<T>(string xml)
        {
            XmlSerializer serializer = new XmlSerializer(typeof(T));

            using (StringReader reader = new StringReader(xml))
            {
                return (T)serializer.Deserialize(reader);
            }
        }

        private async void saveButton_Click(object sender, RoutedEventArgs e)
        {
            TrainData dataObj = new TrainData(INPUT_HEIGHT, INPUT_WIDTH, recognition.getWeights(), trained_spells);
            string toSave = dataObj.ToString();

            // get file pickers and store the file
            FileSavePicker fsp = new FileSavePicker();
            fsp.DefaultFileExtension = ".spells";
            fsp.FileTypeChoices.Add("Spells", new List<string>() { ".spells" });
            StorageFile sf = await fsp.PickSaveFileAsync();
            await Windows.Storage.FileIO.WriteTextAsync(sf, toSave);
        }

        private async void loadButton_Click(object sender, RoutedEventArgs e)
        {
            // open the file with the picker
            FileOpenPicker fop = new FileOpenPicker();
            fop.FileTypeFilter.Add(".spells");
            StorageFile sf = await fop.PickSingleFileAsync();

            // read the file
            string text = await Windows.Storage.FileIO.ReadTextAsync(sf);
            // create train data
            TrainData td = TrainData.FromString(text);

            // deserialize the data
            trained_spells = td.Trained_Sets;
            recognition = new Recognition(INPUT_WIDTH * INPUT_HEIGHT, trained_spells.Count);
            recognition.setWeights(td.NeuralNet.Item1, td.NeuralNet.Item2);
        }

        private async void newSpellButton_Click(object sender, RoutedEventArgs e)
        {
            if (!isTraining)
            {
                FileOpenPicker fop = new FileOpenPicker();
                fop.FileTypeFilter.Add(".png");

                StorageFile sf = await fop.PickSingleFileAsync();

                if (sf != null)
                {
                    string filename = sf.DisplayName;
                    newSpellNameTextBox.Text = filename;
                    Image img = new Image();
                    Uri ur = new Uri("ms-appx:///img/" + sf.Name);
                    (canvas.Background as ImageBrush).ImageSource = new BitmapImage(ur);
                    canvas.UpdateLayout();

                    trainCounter = START_TRAIN_COUNTER;
                    todoTextBlock.Text = "train your spell. Trainings left: " + trainCounter;
                    isTraining = true;
                    currentTrainingData.Clear();
                    currentTrainingPoints.Clear();
                }
            }
            else
            {
                todoTextBlock.Text = "train your last spell to the end first.";
            }
        }

        public static List<T> Randomize<T>(List<T> list)
        {
            List<T> randomizedList = new List<T>();
            Random rnd = new Random();
            while (list.Count > 0)
            {
                int index = rnd.Next(0, list.Count); //pick a random item from the master list
                randomizedList.Add(list[index]); //place it at the end of the randomized list
                list.RemoveAt(index);
            }
            return randomizedList;
        }

        private void trainButton_Click(object sender, RoutedEventArgs e)
        {
            recognition = new Recognition(INPUT_WIDTH * INPUT_HEIGHT, trained_spells.Count);
            saveButton.IsEnabled = true;

            // generate train data
            List<ArrayInputData> trainData = new List<ArrayInputData>();
            for (int i = 0; i < trained_spells.Count; i++)
            {
                DenseVector outVector = new DenseVector(trained_spells.Count, 0);
                // set the correct output index to 1.0
                outVector[i, 0] = 1 ;

                // for all equal training data, add one column
                foreach (var item in trained_spells[i].TrainData)
	            {
                    ArrayInputData aid = new ArrayInputData(item.Value, outVector);
                    trainData.Add(aid);
	            }
            }

            trainData = Randomize(trainData);

            // load it in the recognition object
            recognition.loadTrainData(trainData);

            // and train it
            recognition.train(100);

            testSpellButton.IsEnabled = true;
        }

        private void testSpellButton_Click(object sender, RoutedEventArgs e)
        {
            todoTextBlock.Text = "draw a spell in the window.";
            newSpellButton.IsEnabled = false;
            currentTrainingPoints.Clear();
        }
    }
}
