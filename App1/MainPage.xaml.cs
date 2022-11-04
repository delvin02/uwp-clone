using System;
using System.Collections.Generic;
using System.IO;
using System.Linq;
using System.Runtime.InteropServices.WindowsRuntime;
using Windows.Foundation;
using Windows.Foundation.Collections;
using Windows.UI.Popups;
using Windows.UI.Xaml;
using Windows.UI.Xaml.Controls;
using Windows.UI.Xaml.Controls.Primitives;
using Windows.UI.Xaml.Data;
using Windows.UI.Xaml.Input;
using Windows.UI.Xaml.Media;
using Windows.UI.Xaml.Navigation;

// The Blank Page item template is documented at https://go.microsoft.com/fwlink/?LinkId=402352&clcid=0x409

namespace App1
{
    /// <summary>
    /// An empty page that can be used on its own or navigated to within a Frame.
    /// </summary>
    public sealed partial class MainPage : Page
    {
        public MainPage()
        {
            this.InitializeComponent();
        }

        // Weei Waai Khor
        // 25/10/2022
        //CarSalesV1
        private async void calculate(object sender, RoutedEventArgs e)
        {

            // Name and Phone Number
            bool isName, isHp, isEmptyName, isEmptyHp;
            string name, phoneNumber;

            // Check if name is empty
            isEmptyName = string.IsNullOrEmpty(customerName.Text);

            if (isEmptyName)
            {
                var error_name = new MessageDialog("Name must be filled.");
                await error_name.ShowAsync();
                customerName.Focus(FocusState.Programmatic);
                customerName.SelectAll();
                return;
            }
            // Assign customerName into name
            name = customerName.Text;
            
            // Check name input contains digit
            isName = name.All(char.IsDigit);

            // Display error if True
            if (isName)
            {
                var error_name = new MessageDialog("Name mustn't contains any digit.");
                await error_name.ShowAsync();
                customerName.Focus(FocusState.Programmatic);
                customerName.SelectAll();
                return;
            }

            // Check if phone number is empty
            isEmptyHp = string.IsNullOrEmpty(handphoneNumber.Text);

            // Display Empty/Null Error
            if (isEmptyHp)
            {
                var error_hp = new MessageDialog("Phone Number must be filled.");
                await error_hp.ShowAsync();
                handphoneNumber.Focus(FocusState.Programmatic);
                handphoneNumber.SelectAll();
                return;
            }
            // Assign HP into phoneNumber
            phoneNumber = handphoneNumber.Text;

            // Check name input contains Letter
            isHp = phoneNumber.All(char.IsLetter);

            // Display error if True
            if (isHp)
            {
                var error_hp = new MessageDialog("Phone Number mustn't contains any letter.");
                await error_hp.ShowAsync();
                handphoneNumber.Focus(FocusState.Programmatic);
                handphoneNumber.SelectAll();
                return;
            }

            // Sub Amount Calculation
            double subAmount, carCost, tradeCost;

            try
            {
                // Assign Vechicle Cost and Trade In value
                carCost = double.Parse(vechicleCost.Text);
            }
            catch (Exception exception)
            {
                // Display error
                var dialogMessage = new MessageDialog("Error! Please enter the car cost as float." + exception.Message);
                await dialogMessage.ShowAsync();

                // Select vechicle cost input field
                vechicleCost.Focus(FocusState.Programmatic);
                vechicleCost.SelectAll();
                return;
            }
  

            // Check TradeCost empty of filled boolean
            bool checkTradeCost = string.IsNullOrEmpty(tradein.Text);
            
            if (checkTradeCost)
            {
                // Assign tradeCost variable to 0 at the backend
                tradeCost = 0.0;

                // Set textbox to 0
                tradein.Text = "0";
            }
            else
            {
                try
                {
                    // Assign tradeCost to input value
                    tradeCost = double.Parse(tradein.Text);
                }
                catch (Exception exception)
                {
                    // Display error
                    var tradeCostMessage = new MessageDialog("Error! Please enter the trade cost as float." + exception.Message);
                    await tradeCostMessage.ShowAsync();

                    // Select trade cost input field
                    tradein.Focus(FocusState.Programmatic);
                    tradein.SelectAll();
                    return;
                }
            }

            // Car Insurance
            double insuranceRate;
            int selectedIndex;

            selectedIndex = insurance.SelectedIndex;

            // Check Index to assign insurance rate
            if (selectedIndex == 1)
            {
                insuranceRate = 1.05;
            } else if (selectedIndex == 2)
            {
                insuranceRate = 1.10;
            } else if (selectedIndex == 3)
            {
                insuranceRate = 1.2;
            }
            else
            {
                insuranceRate = 1;
            }

            // Prompt error message when carCost < 0
            if (carCost < 0)
            {
                // Display Error
                var error_name = new MessageDialog("Vehicle Cost must be bigger than 0.");
                await error_name.ShowAsync();

                // Focus on Input field
                vechicleCost.Focus(FocusState.Programmatic);
                vechicleCost.SelectAll();
                return;
            }

            // Prompt error message when tradeCost < 0
            if (tradeCost < 0)
            {
                // Display error
                var error_name = new MessageDialog("Trade Cost must be greater than or equal to 0.");
                await error_name.ShowAsync();
                
                // Focus on Input field
                tradein.Focus(FocusState.Programmatic);
                tradein.SelectAll();
                return;
            }
            // Prompt error message when carCost < tradeCost
            if (carCost < tradeCost)
            {
                // Display Error
                var error_name = new MessageDialog("Vehicle Price must be greater than Trade Cost");
                await error_name.ShowAsync();
                
                // Focus on input field
                vechicleCost.Focus(FocusState.Programmatic);
                vechicleCost.SelectAll();
                return;
            }

            // Calculate Sub amount
            subAmount = carCost - tradeCost;
            subAmountTotal.Text = subAmount.ToString();

            // Total GST
            int gst;
            gst = (int)Math.Round(float.Parse(vechicleCost.Text) * 0.1);
            gstAmountTotal.Text = gst.ToString();

            // Add-ons 
            int extraCost = 0;
            if (window.IsChecked == true)
            {
                extraCost = extraCost + 150;
            }
            if (duco.IsChecked == true)
            {
                extraCost = extraCost + 180;
            }
            if (floor.IsChecked == true)
            {
                extraCost = extraCost + 320;
            }
            if (sound.IsChecked == true)
            {
                extraCost = extraCost + 350;
            }

           

            // Final Amount
            double final;
            final = gst + subAmount + extraCost;
            
            // Display final result
            finalAmountTotal.Text = final.ToString();
            
        }
        private void reset(object sender, RoutedEventArgs e)
        {
            // Clear every input box
            customerName.Text = "";
            vechicleCost.Text = "";
            handphoneNumber.Text = "";
            tradein.Text = "";
            subAmountTotal.Text = "";
            gstAmountTotal.Text = "";
            finalAmountTotal.Text = "";

            // If save button is being clicked 
            customerName.IsEnabled = true;
            handphoneNumber.IsEnabled = true;

            // Focus on customer name - ready for a new customer
            customerName.Focus(FocusState.Programmatic);
        }

        private async void save(object sender, RoutedEventArgs e)
        {

            // Validate Name and HP number
            bool name, phone;
            name = string.IsNullOrEmpty(customerName.Text);
            phone = string.IsNullOrEmpty(handphoneNumber.Text);
            if (name)
            {
                var error_name = new MessageDialog("Name cannot be blank." );
                await error_name.ShowAsync();
                customerName.Focus(FocusState.Programmatic);
                customerName.SelectAll();
                return;
            }
            if (phone)
            {
                var error_name = new MessageDialog("Phone Number cannot be blank.");
                await error_name.ShowAsync();
                handphoneNumber.Focus(FocusState.Programmatic);
                handphoneNumber.SelectAll();
                return;
            }

            customerName.IsEnabled = false;
            handphoneNumber.IsEnabled = false;
            vechicleCost.Focus(FocusState.Programmatic);
        }

        private void ComboBox_SelectionChanged(object sender, SelectionChangedEventArgs e)
        {

        }
    }
}
