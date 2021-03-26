﻿using ReathUIv0._3.Connections;
using ReathUIv0._3.Models;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Text.RegularExpressions;
using System.Threading.Tasks;

namespace ReathUIv0._3.ViewModel
{
    public class InputViewModel
    {

        public string InfoBoxText { get; set; }

        private List<int> idRetrieval = new List<int>();

        private static Regex assetNameRegex = new Regex(@"^[\w\s]{1,200}$");
        private static Regex numbers = new Regex(@"^[0-9]{1,6}$");
        private static Regex decimalNumbers = new Regex(@"^[0-9]{1,6}.[0-9]{2}$");
        private static Regex weightsRegex = new Regex(@"^[0-9]{1,4}.[0-9]{3}$");
        private static Regex recyledPercentReg = new Regex(@"^[0-9]{1,3}$");
        private static Regex mePercentReg = new Regex(@"^[0-9]{1,3}.[0-9]{2}$");

        private static ReusableAsset reusableAsset = new ReusableAsset();

        public InputViewModel()
        {
            InfoBoxText = "Info Box";
        }

        public InputViewModel(ReusableAsset partReusableAsset,string dataSampleSize, string nameOfAsset, string unitCost, string unitWeight, string primaryMaterialWeight, string auxiliarMaterialWeight, string recycledPercent, string mePercent,string primaryMaterialCost,string auxiliaryMaterialCost)
        {

            InfoBoxText = "Info Box";
            
            reusableAsset = partReusableAsset;
            if (CheckAssetDataInputs(dataSampleSize, nameOfAsset, unitCost, unitWeight) == false)
            {
                InfoBoxText = "Part of the Main Asset Information has been entered incorrectly see below for more information. \n" + InfoBoxText;
            }
            else if (CheckMaterialEmissionAndCost(primaryMaterialCost,auxiliaryMaterialCost) == false)
            {
                InfoBoxText = "Part of the Emission and Cost for Materials has been enterd incorrectly see below for more information. \n" + InfoBoxText;
            }
            else if (CheckAssetMaterial(primaryMaterialWeight, auxiliarMaterialWeight) == false)
            {
                InfoBoxText = "Part of the Material section Information has been entered incorrectly see below for more information. \n" + InfoBoxText;
            }
            else if (CheckDisposalMethods() == false)
            {
                InfoBoxText = "Part of the Disposal section Information has not been selected see below for more information. \n" + InfoBoxText;
            }
            else if (CheckRecycleData(recycledPercent) == false)
            {
                InfoBoxText = "Part of the Recycle section Information has been entered incorrectly see below for more information. \n" + InfoBoxText;
            }
            else if (CheckReuseData(mePercent) == false)
            {
                InfoBoxText = "Part of the Reuse section information has been entered incorrectly see below for more information. \n" + InfoBoxText;
            }
            else if (SaveAsset() == false)
            {
                InfoBoxText = "There was a problem saving the Asset to the database. Please try again";
            }
            else
            {
                InfoBoxText = "Asset Successfully saved";
            }
                
        }

        #region Main Asset Data check
        /// <summary>
        /// Checks the main data of the asset being inputted this includes the first three rows 
        /// this consists of the data sample size, data range, name of the asset, unit cost, unit weight and the asset's country of origin
        /// They are checked in that order and if anything doesnt match any of the if statements a error is displayed and false is returned 
        /// Otherwise the specific value will either be saved such as the name of the asset or the other fields will be converted to their specific variable type 
        /// The variable when converted will also then be checked to ensure the value is not 0
        /// The value will then be added to the specific field in the reusableAsset object
        /// </summary>
        /// <param name="dataSampleSize"></param>
        /// <param name="nameOfAsset"></param>
        /// <param name="unitCost"></param>
        /// <param name="unitWeight"></param>
        /// <returns></returns>
        private bool CheckAssetDataInputs(string dataSampleSize,string nameOfAsset,string unitCost,string unitWeight)
        {
            if (dataSampleSize.Length == 0 || numbers.IsMatch(dataSampleSize) == false)
            {
                InfoBoxText = "Nothing has been inputted for the data sample,anything other than a number has been inputted or input is greater than the range allowed. The maximum number allowed is '999999'.";
                return false;
            }
            else
            {
                try
                {
                    int sampleSize = Int32.Parse(dataSampleSize);

                    if(sampleSize  <= 0)
                    {
                        InfoBoxText = "The sample size entered is less than or equal to 0. Please enter a sufficient amount";
                        return false;
                    }
                    else
                    {
                        reusableAsset.SampleSize = sampleSize;
                    }

                }
                catch (FormatException)
                {
                    InfoBoxText = "Error occured formatting Sample size into a integer. Please try again";
                    return false;
                }
            }

            
            if(reusableAsset.DateRange.Length <= 0 || reusableAsset.DateRange.Equals("Date Range of Sample Size") == true)
            {
                InfoBoxText = "The Date Range has not been selected. Please ensure a date range has been selected";
                return false;
            }
            
            if(nameOfAsset.Length == 0 || nameOfAsset.Equals("Name of Asset") || nameOfAsset.Length > 200)
            {
                InfoBoxText = "The name of the asset's length is less than or equal to 0 or the length is greater than 200 characters. Please ensure the asset name has been inputted";
                return false;
            }
            else
            {
                if(assetNameRegex.IsMatch(nameOfAsset) == false)
                {
                    InfoBoxText = "The name of the asset inputted may contain non-word characters. Please ensure there are only word characters and white space if needed.";
                    return false;
                }
                else
                {
                    reusableAsset.AssetName = nameOfAsset;
                }
                
            }

            if(unitCost.Length == 0 || decimalNumbers.IsMatch(unitCost) == false)
            {
                InfoBoxText = "There has been nothing entered into the unit cost box or does not match the format. The format must follow '0.00' being the minimum and '000000.00' being the maximum to enter";
                return false;
            }
            else
            {
                try
                {
                    float unitCosts = float.Parse(unitCost);

                    if(unitCosts <= 0.00)
                    {
                        InfoBoxText = "Unit Cost entered is less than or equal to 0. Please enter a sufficient amount";
                        return false;
                    }
                    else
                    {
                        reusableAsset.UnitCost = unitCosts;
                    }

                }
                catch (FormatException)
                {
                    InfoBoxText = "Error occured formatting unit cost into a float. Please try again";
                    return false;
                }
            }

            if(unitWeight.Length == 0 || weightsRegex.IsMatch(unitWeight) == false)
            {
                InfoBoxText = "Please ensure numbers have been entered or the unit weight entered is in the incorrect format. Format should be '0.000' being the minimum with '0000.000' being the maximum";
                return false;
            }
            else
            {
                try
                {
                    float unitsWeight = float.Parse(unitWeight);
                    

                    if(unitsWeight <= 0.00)
                    {
                        InfoBoxText = "Unit weight entered is less than or equal to 0.  Please enter a sufficient amount";
                        return false;
                    }
                    else
                    {
                        reusableAsset.UnitWeight = unitsWeight;
                    }

                }
                catch (FormatException)
                {
                    InfoBoxText = "Error occured formatting unit weight into a float. Please try again";
                    return false;
                }
            }

            if(string.IsNullOrEmpty(reusableAsset.AssetCountryOfOrigin) == true)
            {
                InfoBoxText = "No country selected for the Assets country of origin. Please select a country";
                return false;
            }


            return true;
        }
        #endregion

        private bool CheckMaterialEmissionAndCost(string primaryMaterialCost,string auxiliaryMaterialCost)
        {
            if (primaryMaterialCost.Length == 0 || decimalNumbers.IsMatch(primaryMaterialCost) == false)
            {
                InfoBoxText = "There has been nothing entered into the Primary Material cost box or does not match the format. The format must follow '0.00' being the minimum and '000000.00' being the maximum to enter";
                return false;
            }
            else
            {
                try
                {
                    float primMatCost = float.Parse(primaryMaterialCost);

                    if (primMatCost <= 0.00)
                    {
                        InfoBoxText = "Primary Material Cost entered is less than or equal to 0. Please enter a sufficient amount";
                        return false;
                    }
                    else
                    {
                        reusableAsset.PrimaryMaterialCost = primMatCost;
                    }

                }
                catch (FormatException)
                {
                    InfoBoxText = "Error occured formatting primary material cost into a float. Please try again";
                    return false;
                }
            }

            if (auxiliaryMaterialCost.Length == 0 || decimalNumbers.IsMatch(primaryMaterialCost) == false && reusableAsset.AuxiliaryMaterial != null)
            {
                InfoBoxText = "There has been nothing entered into the Auxiliary Material cost box or does not match the format. The format must follow '0.00' being the minimum and '000000.00' being the maximum to enter";
                return false;
            }
            else
            {
                try
                {
                    float auxMatCost = float.Parse(auxiliaryMaterialCost);

                    if (auxMatCost <= 0.00)
                    {
                        InfoBoxText = "Auxiliary Material Cost entered is less than or equal to 0. Please enter a sufficient amount";
                        return false;
                    }
                    else
                    {
                        reusableAsset.AuxiliaryMaterialCost = auxMatCost;
                    }

                }
                catch (FormatException)
                {
                    InfoBoxText = "Error occured formatting auxiliary material cost into a float. Please try again";
                    return false;
                }
            }

            if (reusableAsset.PrimaryMaterialEmission.Length == 0)
            {
                InfoBoxText = "No Primary Emission method has been selected. Please select a Primary Emission method";
                return false;
            }


            if (reusableAsset.AuxiliaryMaterialEmission.Length == 0 && string.IsNullOrEmpty(reusableAsset.AuxiliaryMaterial) == false)
            {
                InfoBoxText = "No Auxiliary Emission method has been selected. Please select a Auxiliary Emission method";
                return false;
            }

            if (reusableAsset.PrimaryMaterialEmission.Length == 0 && string.IsNullOrEmpty(reusableAsset.PrimaryMaterial) == false)
            {
                InfoBoxText = "No Primary Emission method has been selected. Please select a Primary Emission method";
                return false;
            }

            return true;
        }

        #region Asset Material Check
        /// <summary>
        /// This checks the assets material section which is the fourth row
        /// This includes the primary material, primary material weight, auxiliar material and auxiliar weight
        /// Same as above they are checked in the order from left to right
        /// First making sure the primary material box has a selected item
        /// Then checks the primary weight ensureing it matches the regex then assigns the value in the reusableAsset object
        /// Then checks if the auxiliar weight is inputted without a auxiliar material selected producing a error if so
        /// Checks to make sure the auxiliar weight matches the regex if so it is then compared to the primary material weight to ensure it is less than it 
        /// If less than the primary weight it will be assigned the value in the reusableAsset object
        /// </summary>
        /// <param name="primaryMaterialWeight"></param>
        /// <param name="auxiliarMaterialWeight"></param>
        /// <returns></returns>
        private bool CheckAssetMaterial(string primaryMaterialWeight, string auxiliarMaterialWeight)
        {
            if(string.IsNullOrEmpty(reusableAsset.PrimaryMaterial) == true)
            {
                InfoBoxText = "No Primary Material Selected. Please ensure a Primary Material is selected";
                return false;
            }

            if(primaryMaterialWeight.Length <= 0 || weightsRegex.IsMatch(primaryMaterialWeight) == false)
            {
                InfoBoxText = "Primary material weight either hasn't been entered or has been inputted incorrectly. The format should be 0.000 but should not be less than or equal to 0";
                return false;
            }
            else
            {
                try
                {
                    float primaryWeight = float.Parse(primaryMaterialWeight);

                    if(primaryWeight <= 0)
                    {
                        InfoBoxText = "Primary material weight inputted is less than or equal to 0. Please input a sufficient number";
                        return false;
                    }
                    else
                    {
                        reusableAsset.PrimaryWeight = primaryWeight;
                    }
                }
                catch (FormatException)
                {
                    InfoBoxText = "Error occured formatting primary weight to float. Please try again";
                    return false;
                }
                
            }

            if(string.IsNullOrEmpty(reusableAsset.AuxiliaryMaterial) == true && !auxiliarMaterialWeight.Equals("Aux Weight") && auxiliarMaterialWeight.Length > 0)
            {
                InfoBoxText = "You have inputted data into the auxiliar weight input but no auxiliar material selected or have removed the aux weight from the input box either remove everything from the auxiliar weight box or retype 'Aux Weight'.";
                return false;
            }


            if(string.IsNullOrEmpty(reusableAsset.AuxiliaryMaterial) == false && auxiliarMaterialWeight.Length > 0)
            {
                
                if(weightsRegex.IsMatch(auxiliarMaterialWeight) == true)
                {
                    try
                    {
                        float auxiliarWeight = float.Parse(auxiliarMaterialWeight);

                        if(auxiliarWeight == reusableAsset.PrimaryWeight || auxiliarWeight < reusableAsset.PrimaryWeight)
                        {
                            float temp = 0.00F;
                                
                            temp = reusableAsset.PrimaryWeight + auxiliarWeight;

                            if(temp == reusableAsset.UnitWeight)
                            {
                                reusableAsset.AuxiliaryWeight = auxiliarWeight;
                                return true;
                            }
                            else
                            {
                                InfoBoxText = "The inputted weights of both primary and auxiliar do not equal unit weight. Please try again."+ "\n Auxiliar Weight: " + auxiliarWeight  + "\n Primary Weight: " + reusableAsset.PrimaryWeight + "\n Combined Weight: " + temp;
                                return false;
                            }
                        }
                        else
                        {

                            InfoBoxText = "Auxiliar weight is greater than Primary weight or weights to do not equal unit weight. Please input weight correctly.";
                            return false;
                        }
                    }
                    catch (FormatException)
                    {
                        InfoBoxText = "Error occured formatting auxiliar weight to float. Please try again";
                        return false;
                    }
                }
                else
                {
                    InfoBoxText = "Auxiliar weight inputted incorrectly please ensure the format being '0.000'. Please try again";
                    return false;
                }
            }
            else if(string.IsNullOrEmpty(reusableAsset.AuxiliaryMaterial) == false && string.IsNullOrEmpty(auxiliarMaterialWeight) == true)
            {
                InfoBoxText = "Please ensure a Weight has been entered if a Auxiliar material has been selected";
                return false;
            }


            if (reusableAsset.PrimaryWeight != reusableAsset.UnitWeight)
            {
                InfoBoxText = "Primary Weight is not the same as unit weight. Please input the weight correctly";
                return false;
            }

            return true;
        }
        #endregion

        #region Check Disposal 
        /// <summary>
        /// Used to check the disposal methods for Primary and auxiliary if there is a auxiliary
        /// </summary>
        /// <returns></returns>
        private bool CheckDisposalMethods()
        {
            if (reusableAsset.PrimaryDispoMethod.Length == 0)
            {
                InfoBoxText = "No Primary disposal method has been selected. Please select a Primary disposal method";
                return false;
            }

            if (reusableAsset.PrimaryCleaningMethod.Length == 0)
            {
                InfoBoxText = "No cleaning method has been selected. Please select a cleaning method";
                return false;
            }

            if (reusableAsset.AuxiliaryDispoMethod.Length == 0 && string.IsNullOrEmpty(reusableAsset.AuxiliaryMaterial) == false)
            {
                InfoBoxText = "No Auxiliary disposal method has been selected. Please select a Auxiliary disposal method";
                return false;
            }

            if (reusableAsset.AuxiliaryCleaningMethod.Length == 0 && string.IsNullOrEmpty(reusableAsset.AuxiliaryMaterial) == false)
            {
                InfoBoxText = "No Auxiliary cleaning method has been selected. Please select a Auxiliary cleaning method";
                return false;
            }

            return true;
        }
        #endregion

        #region Recycle Check
        /// <summary>
        /// First checks to see if the item is being recycled or not 
        /// Will produce a error if nothing selected of if set to 0 will return true 
        /// If 1 will check the other fields in the same row being the fifth row
        /// First checking the recycle percent making sure a number has been entered and the it isnt less than  or equal to 0 or greater than 100 will then be assigned to the value in the reusabelAsset object
        /// then checks the country of origin making sure it has a selected value will then do the same with the cleaning method
        /// Will return true if everything is good
        /// </summary>
        /// <param name="recycledPercent"></param>
        /// <param name="mePercent"></param>
        /// <returns></returns>
        private bool CheckRecycleData(string recycledPercent)
        {
            if (reusableAsset.IsRecylced == 2)
            {
                InfoBoxText = "If the asset has been recycled has not been selected. Please ensure that either yes or no has been selected ";
                return false;
            }
            else if(reusableAsset.IsRecylced == 1)
            {
                if(recycledPercent.Length == 0 || recyledPercentReg.IsMatch(recycledPercent) == false)
                {
                    InfoBoxText = "Please ensure the Recycled percent has an input if the item is being recycled and also ensure a number has been inputted. Please try again";
                    return false;
                }
                else
                {
                    try
                    {
                        int recyclePercent = Int32.Parse(recycledPercent);

                        if(recyclePercent <= 0 || recyclePercent > 100)
                        {
                            InfoBoxText = "Please ensure the number inputted is greater than 0 but no greater than 100.";
                            return false;
                        }
                        else
                        {
                            reusableAsset.RecycledPercentage = recyclePercent;
                        }
                    }
                    catch (FormatException)
                    {
                        InfoBoxText = "Error occured formatting the recycled percentage. Please try again";
                        return false;
                    }
                }

                if(reusableAsset.RecycledCountryOfOrigin.Length == 0)
                {
                    InfoBoxText = "No country has been selected for where the recycling occurs. Please select a country";
                    return false;
                }

                return true;
                
            }

            return true;
            
        }
        #endregion

        #region Reuse Check
        /// <summary>
        /// Same as the rest checking the reuse data section this time consisting of the last two rows 
        /// First making sure all dropdowns have an item selected otherwise a specific error is produced
        /// Will then check the ME Percent box making sure it has a value entered and matches the regex
        /// When converted to float will make sure it is not less than 0 or greater than 100 
        /// The value is then assigned to the reusableAsset object and true is returned
        /// </summary>
        /// <param name="mePercent"></param>
        /// <returns></returns>
        private bool CheckReuseData(string mePercent)
        {
            if(reusableAsset.ReuseOccurence.Length == 0)
            {
                InfoBoxText = "No reuse time cycle has been selected. Please select a time cycle.";
                return false;
            }

            if (reusableAsset.AverageDistanceToReuse == 0)
            {
                InfoBoxText = "No average distance to recycle selected. Please select the average distance from the dropdown";
                return false;
            }

            if(reusableAsset.MaximumReuses == 0)
            {
                InfoBoxText = "Number of the maximum reuses has not been selected. Please select a maximum reuse amount";
                return false;
            }

            if(mePercent.Length == 0 || mePercentReg.IsMatch(mePercent) == false)
            {
                InfoBoxText = "The ME Percent either has nothing inputted or does not match the format. Please ensure when entering the ME Percent the format is '0.00' but not less than 0.00 or greater than 100.00";
                return false;
            }
            else
            {
                try
                {
                    float manufacturingCarbonPercent = float.Parse(mePercent);

                    if(manufacturingCarbonPercent < 0 || manufacturingCarbonPercent > 100)
                    {
                        InfoBoxText = "Please ensure the ME Percent entered is not less than 0 or greater than 100";
                        return false;
                    }
                    else
                    {
                        reusableAsset.PercentageOfManufacturingCarbon = manufacturingCarbonPercent;
                        return true;
                    }
                }
                catch (FormatException)
                {
                    InfoBoxText = "Error occured converting ME Percent to float. Please try again";
                    return false;
                }
            }
        }
        #endregion

        #region Saves Asset
        /// <summary>
        /// First will save the recycle data into the recycle table
        /// retrieves the necessary id's for the Reusable Asset table in the database
        /// returning true if everything is good success message will then be displayed shown at the bottom of if else statement
        /// </summary>
        /// <returns></returns>
        private bool SaveAsset()
        {

            if (SqliteDatabaseAccess.SaveRecyle(reusableAsset) != true)
            {
                InfoBoxText = "Error occured while attempting to insert recycle data into the database please try again.";
                return false;
            }

            int primaryId, auxiliarId, recycleId, reusableAssetId;

            idRetrieval = SqliteDatabaseAccess.RetrieveMaterialId(reusableAsset.PrimaryMaterial);

            primaryId = idRetrieval[0];

            idRetrieval = SqliteDatabaseAccess.RetrieveMaterialId(reusableAsset.AuxiliaryMaterial);

            auxiliarId = idRetrieval[0];

            if(auxiliarId == 0)
            {
             
            }

            idRetrieval = SqliteDatabaseAccess.RetrieveRecycleId();

            recycleId = idRetrieval[0];

            if(SqliteDatabaseAccess.SaveReusableAsset(reusableAsset,recycleId) != true)
            {
                InfoBoxText = "Error occured while attempting to insert Reusable Asset into the database please try again.";
                return false;
            }

            idRetrieval = SqliteDatabaseAccess.RetreiveReusableAssetId();

            reusableAssetId = idRetrieval[0];

            if(SqliteDatabaseAccess.SaveReusableAssetManufacturing(reusableAssetId,primaryId,auxiliarId) != true)
            {
                InfoBoxText = "Error occured while attempting to insert the Reusable Asset id and the manufacturing id's into the database. Please try again";
                return false;
            }


            return true;

        }
        #endregion
    }
}