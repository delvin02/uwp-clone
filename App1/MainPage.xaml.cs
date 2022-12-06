using System;
using System.Collections.Generic;
using System.IO;
using System.Linq;
using System.Runtime.InteropServices.WindowsRuntime;
using System.Xml.Linq;
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
using System.Collections.Generic;
using System.Collections.Specialized;
using System.Collections;

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

        // Loaded
        private ArrayList customerNameList = null;
        private ArrayList customerPhoneList = null;

        public ArrayList vehicleMakes = null;

        private ArrayList loadDictionaryCustomerName()
        {
            ArrayList customerNameList = new ArrayList();

            // Customer Name
            customerNameList.Add("John");
            customerNameList.Add("Ann");
            customerNameList.Add("Jordan");
            customerNameList.Add("Arriel");
            customerNameList.Add("Andrew");
            customerNameList.Add("Julie");
            customerNameList.Add("Barrel");
            customerNameList.Add("Carol");
            customerNameList.Add("Dan");
            customerNameList.Add("Eren");

            return customerNameList;
        }
        
        private ArrayList loadDictionaryCustomerPhone()
        {
            ArrayList customerPhoneList = new ArrayList();

            // Phone list
            customerPhoneList.Add("478 643 011");
            customerPhoneList.Add("478 643 012");
            customerPhoneList.Add("478 643 013");
            customerPhoneList.Add("478 643 014");
            customerPhoneList.Add("478 643 015");
            customerPhoneList.Add("478 643 016");
            customerPhoneList.Add("478 643 017");
            customerPhoneList.Add("478 643 018");
            customerPhoneList.Add("478 643 019");
            customerPhoneList.Add("478 643 020");

            return customerPhoneList;


        }
        private ArrayList loadVehicleMakes()
        {
            ArrayList brands = new ArrayList();
            brands.Add("Toyota");
            brands.Add("Holden");
            brands.Add("Mitsubushi");
            brands.Add("Ford");
            brands.Add("BMW");
            brands.Add("Mazda");
            brands.Add("Volkswagen");
            brands.Add("Mini");
            return brands;
        }
        protected void Page_Loaded(object sender, RoutedEventArgs e)
        {
            // Get data in dictionary form (name => number)
            customerNameList = loadDictionaryCustomerName();
            customerPhoneList = loadDictionaryCustomerPhone();

            vehicleMakes = loadVehicleMakes();

        }
        private async void calculate(object sender, RoutedEventArgs e)
        {
            const double GST_RATE = 0.1;

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

            // Car Warranty
            double warrantyAmount = calcVehicleWarranty(carCost);

            // Add-ons 
            double extraCost = calcOptionalExtras();

            // Car Insurance
            double insuranceAmount = calcAccidentInsurance(carCost, extraCost);

            // Calculate Sub amount
            subAmount = carCost + warrantyAmount + extraCost + insuranceAmount - tradeCost;
            subAmountTotal.Text = "$ " + subAmount.ToString();

            // Total GST
            double gst;
            gst = subAmount * GST_RATE;
            gstAmountTotal.Text = "$ " + gst.ToString();

            // Final Amount
            double final;
            final = subAmount + gst;

            // Display final result
            finalAmountTotal.Text = "$ " + final.ToString();


            // Display in Summary Data Block
            customerNameBlock.Text = "Customer Name: " + name.ToString();
            phoneNumberBlock.Text = "Phone Number: " + phoneNumber.ToString();
            vehicleCostBlock.Text = "Vehicle Cost: $" + carCost.ToString();
            tradeInBlock.Text = "Trade In: $" + tradeCost.ToString();
            warrantyCostBlock.Text = "Warranty Cost: $" + warrantyAmount.ToString();
            extraCostBlock.Text = "Extra Cost: $" + extraCost.ToString();
            insuranceCostBlock.Text = "Insurance Cost: $" + insuranceAmount.ToString();
            finalAmountBlock.Text = "Final Amount: $" + final.ToString();


        }

        /// <summary>
        /// Calculate Vehicle Warranty
        /// </summary>
        /// <param name="vehiclePrice"></param>
        /// <returns>warraty rate * vehiclePrice</returns>

        // Warranty 

        private double calcVehicleWarranty(double vehiclePrice)
        {
            const double WARRANTY1 = 0.05;
            const double WARRANTY2 = 0.10;
            const double WARRANTY3 = 0.20;

            double warrantyRate;
            int selectedIndex;

            selectedIndex = warranty.SelectedIndex;

            // Check Index to assign insurance rate
            switch (selectedIndex)
            {
                case 1:
                    warrantyRate = WARRANTY1;
                    break;
                case 2:
                    warrantyRate = WARRANTY2;
                    break;
                case 3:
                    warrantyRate = WARRANTY3;
                    break;
                default:
                    return 0;
            }
            return vehiclePrice * warrantyRate;
        }

        /// <summary>
        /// Extras Addons Amount
        /// </summary>
        /// <returns>extraCost</returns>
        /// 
        // Addons
        private double calcOptionalExtras()
        {
            double extraCost = 0.0;

            const double WINDOW_PRICE = 150.0;
            const double DUCO_PRICE = 180.0;
            const double FLOOR_PRICE = 320.0;
            const double SOUND_PRICE = 350.0;
            // check window
            if (window.IsChecked == true)
            {
                extraCost = extraCost + WINDOW_PRICE;
            }
            // check duco
            if (duco.IsChecked == true)
            {
                extraCost = extraCost + DUCO_PRICE;
            }
            // check floor
            if (floor.IsChecked == true)
            {
                extraCost = extraCost + FLOOR_PRICE;
            }
            // check sound
            if (sound.IsChecked == true)
            {
                extraCost = extraCost + SOUND_PRICE;
            }
            return extraCost;
        }

        // Toggle Insurance Switch
        private void InsuranceSwitch_Toggled(object sender, RoutedEventArgs e)
        {
            ToggleSwitch toggleSwitch = sender as ToggleSwitch;


            // Toggle Radio Button
            if (toggleSwitch != null)
            {
                if (toggleSwitch.IsOn == true)
                {
                    // Make visible
                    AgeUnder.Visibility = Visibility.Visible;
                    AgeAbove.Visibility = Visibility.Visible;
                }
                else
                {
                    // Make invisible
                    AgeUnder.Visibility = Visibility.Collapsed;
                    AgeAbove.Visibility = Visibility.Collapsed;
                }
            }
        }
        /// <summary>
        ///     double rate
        ///     Identify which button is checked
        ///         rate = 0.2
        /// </summary>
        /// <param name="vehiclePrice"></param>
        /// <param name="optionalExtras"></param>
        /// <returns> (vehicleprice * rate) + optionalExtras</returns>

        // Calculate Insurance
        private double calcAccidentInsurance(double vehiclePrice, double optionalExtras)
        {
            const double INSURANCE_RATE_BELOW = 0.2;
            const double INSURANCE_RATE_ABOVE = 0.1;

            double total;
            // Check Toggle is On
            if (insuranceToggle.IsOn == true)
            {
                // Check either button is checked
                if (AgeUnder.IsChecked == true)
                {
                    total = (vehiclePrice + optionalExtras) * INSURANCE_RATE_BELOW;
                }
                else if (AgeAbove.IsChecked == true)
                {
                    total = (vehiclePrice + optionalExtras) * INSURANCE_RATE_ABOVE;
                }
                else
                {
                    return 0;
                }
            }
            else
            {
                return 0;
            }
            return total;
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

            // Optional Extras
            window.IsChecked = false;
            duco.IsChecked = false;
            floor.IsChecked = false;
            sound.IsChecked = false;

            // Warranty
            warranty.SelectedIndex = 0;

            // Insurance Toggle
            insuranceToggle.IsOn = false;

            // Reset Display Data
            customerNameBlock.Text = "Customer Name";
            phoneNumberBlock.Text = "Phone Number";
            vehicleCostBlock.Text = "Vehicle Cost";
            tradeInBlock.Text = "Trade In";
            warrantyCostBlock.Text = "Warranty Cost";
            extraCostBlock.Text = "Optional Extras Cost";
            insuranceCostBlock.Text = "Insurance Cost";
            finalAmountBlock.Text = "Final Amount";

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
                var error_name = new MessageDialog("Name cannot be blank.");
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

        
        private void displayNamesButton_Click(object sender, RoutedEventArgs e)
        {
            ResultTextBlock.Text = "";

            // Same size array (Customer and Phone)
            for (int i = 0; i < customerNameList.Count; i++)
            {
                ResultTextBlock.Text = ResultTextBlock.Text + customerNameList[i] + ": " + customerPhoneList[i] + "\n";
            }

        }

        /// <summary>
        /// Sequential search - Linear search
        /// Loop every elements in an array
        ///     Compare each element until it finds a match
        ///     break
        /// </summary>
        /// <param name="sender"></param>
        /// <param name="e"></param>
        private async void searchNameButton_Click(object sender, RoutedEventArgs e)
        {
            // get input from user
            bool isEmptyTextBox;
            isEmptyTextBox = string.IsNullOrEmpty(customerName.Text);

            if (isEmptyTextBox)
            {
                var blankTextError = new MessageDialog("Text box must be filled.");
                await blankTextError.ShowAsync();
                customerName.Focus(FocusState.Programmatic);
                customerName.SelectAll();
                return;
            }

            displayNamesButton_Click(sender, e);

            var customerNameInput = customerName.Text;

            // Keep foundUser false for the moment
            bool foundUser = false;

            // Check customer name using loop
            int i = 0;
            while (!foundUser && i < customerNameList.Count)
            {

                /// ***
                if (customerNameList[i].ToString().ToLower() == customerNameInput.ToLower())
                {
                    ResultTextBlock.Text = (String)customerPhoneList[i];

                    customerName.Text = customerNameList[i].ToString();
                    handphoneNumber.Text = customerPhoneList[i].ToString();
                    // User found
                    foundUser = true;
                }
                i++;
            }

            // Remain not found
            if (foundUser == false)
            {
                var notFound = new MessageDialog(customerName + " cannot be found");
                await notFound.ShowAsync();

                customerName.Focus(FocusState.Programmatic);
                customerName.SelectAll();
            }
        }


        private async void deleteNameButton_Click(object sender, RoutedEventArgs e)
        {
            //Check TextBox is empty
            bool isEmptyTextBox;
            isEmptyTextBox = string.IsNullOrEmpty(customerName.Text);

            if (isEmptyTextBox)
            {
                //Blank Text Error
                var blankTextError = new MessageDialog("Text box must be filled.");
                await blankTextError.ShowAsync();
                customerName.Focus(FocusState.Programmatic);
                customerName.SelectAll();
                return;
            }

            //Get input from textbox
            var customerNameInput = customerName.Text;

            for (int i = 0; i < customerNameList.Count; i++)
            {
                //If key item is equal to inputted name
                if ((string)customerNameList[i] == customerNameInput)
                {
                    //try to remove from array
                    try
                    {
                        // Remove Name and Phone
                        customerNameList.Remove(customerNameList[i]);
                        customerPhoneList.Remove(customerPhoneList[i]);

                        UserInputTextBox.Text = "";

                        customerName.Text = "";
                        handphoneNumber.Text = "";

                        var deleteSuccess = new MessageDialog(customerName + " HP num:" + customerPhoneList[i] + " deleted successfully." + "Current Array Length:" + customerNameList.Count);
                        await deleteSuccess.ShowAsync();
                        displayNamesButton_Click(sender, e);
                        return;
                    }
                    //Deal with exception
                    catch (Exception exception)
                    {
                        var deleteError = new MessageDialog("Error" + exception.Message);
                        await deleteError.ShowAsync();
                        return;
                    }
                }
            }
            //Delete failed
            //DisplayNames_Click(sender, e);
            var deleteFailed = new MessageDialog(customerName + " Not found.");
            await deleteFailed.ShowAsync();
        }

        protected void DisplayAllMakesButton_Click(object sender, RoutedEventArgs e)
        {
            string[] brands = (string[])vehicleMakes.ToArray(typeof(string));

            //sort array
            Array.Sort(brands);

            //reset text block
            ResultTextBlock.Text = "";

            //loop through items in brands
            foreach (var brand in brands)
            {
                ResultTextBlock.Text = ResultTextBlock.Text + brand + "\n";
            }

        }
        /// <summary>
        /// Binary Search - Divide and Conquer Method
        /// While is not end of the list
        ///     Repeatedly diving in half the portion of the list that could contain the item
        /// </summary>
        /// <param name="sender"></param>
        /// <param name="e"></param>
        protected async void BinarySearchButton_Click(object sender, RoutedEventArgs e)
        {
            //Check TextBox is empty
            bool isEmptyTextBox;
            isEmptyTextBox = string.IsNullOrEmpty(UserInputTextBox.Text);

            if (isEmptyTextBox)
            {
                //Blank text error
                var blankTextError = new MessageDialog("Text box must be filled.");
                await blankTextError.ShowAsync();
                UserInputTextBox.Focus(FocusState.Programmatic);
                UserInputTextBox.SelectAll();
                return;
            }

            //Get input from textbox
            var vehicleBrand = UserInputTextBox.Text.ToUpper();

            String[] brands = (String[])vehicleMakes.ToArray(typeof(string));
            Array.Sort(brands);

            //Minimum, Maximum number for binary search (divide & conquer)

            int minNum = 0;
            int maxNum = brands.Length - 1;

            //Do - While Loop
            do
            {
                int mid = (minNum + maxNum) / 2;

                //If vehicle name is found
                if (vehicleBrand == brands[mid].ToUpper())
                {
                    //Successful Message
                    var foundMessage = new MessageDialog(brands[mid] + " was found, indexed at " + mid);
                    await foundMessage.ShowAsync();

                    UserInputTextBox.Text = "";
                    //Exit Program
                    return;
                }
                //Check if the item is bigger than mid or lesser than mid
                if (vehicleBrand.CompareTo(brands[mid].ToUpper()) > 0)
                    //if bigger than mid, change minimum number for then next divide & conquer method

                   minNum = mid + 1;
                else
                     //if smaller than mid, change maximum number for the next divide & conquer method

                   maxNum = mid - 1;

            } while (minNum <= maxNum);
            //Error Meesage
            var notFound = new MessageDialog(vehicleBrand + " cannot be found.");
            await notFound.ShowAsync();
        }
        protected async void InsertVehicleButton_Click(object sender, RoutedEventArgs e)
        {

            //Check TextBox is empty
            bool isEmptyTextBox;
            isEmptyTextBox = string.IsNullOrEmpty(UserInputTextBox.Text);

            if (isEmptyTextBox)
            {
                // Blank text error
                var blankTextError = new MessageDialog("Text box must be filled.");
                await blankTextError.ShowAsync();
                UserInputTextBox.Focus(FocusState.Programmatic);
                UserInputTextBox.SelectAll();
                return;
            }

            // Get input from textbox
            var vehicleBrand = UserInputTextBox.Text;

            String[] brands = (String[])vehicleMakes.ToArray(typeof(string));

            //Compare while ignoring case
            if (brands.Contains(vehicleBrand, StringComparer.OrdinalIgnoreCase))
            {
                // Already Exist error
                var existError = new MessageDialog(vehicleBrand + " already exists.");
                await existError.ShowAsync();
            }
            else
            {
                vehicleMakes.Add(vehicleBrand);
                UserInputTextBox.Text = "";
            }
            DisplayAllMakesButton_Click(sender, e);
        }
    }

}