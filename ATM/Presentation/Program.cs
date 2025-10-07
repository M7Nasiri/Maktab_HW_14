using ATM.Application.Interface;
using ATM.Application.Service;
using ATM.Application.Utils;
using ATM.Domain.Dto;
using ATM.Domain.Exceptions;

ICardService _cardService = new CardService();
ITransactionService _transactionService = new TransactionService();
_transactionService.Init();
bool innerFlag = true;
bool outerFlag = true;
int failedAttempt = 0;
do
{
    Console.Clear();
    Console.WriteLine("Enter Command");
    Console.WriteLine("1-Card To Card");
    Console.WriteLine("2-Change Password");
    Console.WriteLine("3-Reporting Transaction");
    Console.WriteLine("0-Exit");
    int choice = Int32.Parse(Console.ReadLine());
    switch (choice)
    {
        case 1:
            try
            {
                Console.WriteLine("Enter Card Number");
                string sourceCardNumber = Console.ReadLine();
                if (_cardService.IsValidCardNumer(sourceCardNumber))
                {
                    if (!_cardService.IsCardExist(sourceCardNumber))
                    {
                        throw new CardIsNotExistException("Card is not Exist");
                    }
                    else
                    {
                        Console.WriteLine("Enter Password");
                        string password = Console.ReadLine();
                        if (!_cardService.Login(sourceCardNumber, password))
                        {
                            failedAttempt++;
                            _cardService.UpdateFailAttempts(sourceCardNumber, failedAttempt);
                            if (failedAttempt == 3)
                            {
                                if (_cardService.BlockedCard(sourceCardNumber))
                                    Console.WriteLine("Card Blocked");
                                else
                                {
                                    Console.WriteLine("Error on blocked Card");
                                }
                            }
                            throw new PasswordIncorrectException("Password is incorrect");
                        }
                        else
                        {

                            if (!(_cardService.IsCardActive(sourceCardNumber)))
                            {
                                throw new CardIsNotActiveException("Card is not Active");
                            }
                            else
                            {
                                _cardService.UpdateFailAttempts(sourceCardNumber, 0);
                                Console.WriteLine("Enter Destination Card number");
                                string destinationCardNumber = Console.ReadLine();
                                if (!(_cardService.IsCardExist(destinationCardNumber)))
                                {
                                    throw new CardIsNotExistException("Destination card is not Exist");
                                }
                                else
                                {
                                    Console.WriteLine($"Holder Name" +
                                        $" {_cardService.ShowCardHolderName(destinationCardNumber)} \nIs Confirmed?");
                                    bool confirmed = Convert.ToBoolean(Int32.Parse(Console.ReadLine()));
                                    if (confirmed)
                                    {
                                            _cardService.AssignGenerationCode(sourceCardNumber);
                                            Console.WriteLine("Enter Verification Code");
                                            string code = Console.ReadLine();
                                            if (_cardService.ApproveVerificationCode(sourceCardNumber, code))
                                            {
                                            _cardService.StopTimer();
                                                if (!(_cardService.IsCardActive(destinationCardNumber)))
                                                {
                                                    throw new CardIsNotActiveException("Destination Card is not Active");
                                                }
                                                else
                                                {
                                                    Console.WriteLine("Enter amount");
                                                    float amount = float.Parse(Console.ReadLine());
                                                    if (_cardService.IsValidAmount(amount))
                                                    {
                                                        if (_cardService.HasEnoughMoney(sourceCardNumber, amount))
                                                        {
                                                            if (_transactionService.IsReachedDailyTransferLimit(sourceCardNumber, amount))
                                                            {
                                                                if (_cardService.CardToCard(sourceCardNumber, destinationCardNumber, amount))
                                                                {
                                                                    if (_transactionService.CreateTransaction(new CreateTransactionDto
                                                                    {
                                                                        SourceCardNumber = sourceCardNumber,
                                                                        Amount = amount,
                                                                        DestinationCardNumber = destinationCardNumber,
                                                                        IsSuccessfull = true,
                                                                        TransactionDate = DateTime.Now
                                                                    }))
                                                                    {
                                                                        Console.WriteLine("Successfull Transaction!");
                                                                    }
                                                                    else
                                                                    {
                                                                        
                                                                        Console.WriteLine("Error on transaction!");
                                                                    }
                                                                }
                                                                else
                                                                {
                                                                    _transactionService.CreateTransaction(new CreateTransactionDto
                                                                    {
                                                                        SourceCardNumber = sourceCardNumber,
                                                                        Amount = amount,
                                                                        DestinationCardNumber = destinationCardNumber,
                                                                        IsSuccessfull = false,
                                                                        TransactionDate = DateTime.Now
                                                                    });
                                                                    Console.WriteLine("Error on card to card operation!");
                                                                }
                                                            }
                                                            else
                                                            {
                                                                Console.WriteLine("You have reached daily transfer Limit");
                                                            }

                                                        }
                                                        else
                                                        {
                                                            Console.WriteLine("You have not enough money!");
                                                        }
                                                    }
                                                    else
                                                    {
                                                        Console.WriteLine("Please enter valid amount!");
                                                    }
                                                }
                                            }
                                            else
                                            {
                                                Console.WriteLine("Incorrect verification code");
                                            }
                                    }
                                    else
                                    {
                                        Console.WriteLine("Holder name is not confirmed!");
                                    }
                                }
                            }
                            //}
                            //else
                            //{
                            //   Console.WriteLine("Incorrect verification code");
                            //}

                        }
                    }
                }
                else
                {
                    Console.WriteLine("Enter Valid card number in 16 digit!");
                }

            }
            catch (CardIsNotExistException ex)
            {
                Console.WriteLine(ex.Message);
            }
            catch (PasswordIncorrectException ex)
            {
                Console.WriteLine(ex.Message);
            }
            catch (CardIsNotActiveException ex)
            {
                Console.WriteLine(ex.Message);
            }
            catch (Exception ex)
            {
                Console.WriteLine(ex.Message);
            }
            Console.ReadKey();
            break;

        case 2:
            try
            {
                Console.WriteLine("Enter Card Number");
                string sourceCardNumber1 = Console.ReadLine();
                if (_cardService.IsValidCardNumer(sourceCardNumber1))
                {
                    if (!_cardService.IsCardExist(sourceCardNumber1))
                    {
                        throw new CardIsNotExistException("Card is not Exist");
                    }
                    else
                    {
                        Console.WriteLine("Enter Password");
                        string password1 = Console.ReadLine();
                        if (!_cardService.Login(sourceCardNumber1, password1))
                        {
                            throw new PasswordIncorrectException("Password is incorrect");
                        }
                        else
                        {
                            if (!(_cardService.IsCardActive(sourceCardNumber1)))
                            {
                                throw new CardIsNotActiveException("Card is not Active");
                            }
                            else
                            {
                                Console.WriteLine("Enter new Password");
                                string newPass = Console.ReadLine();
                                if (_cardService.ChangeCardPassword(sourceCardNumber1, password1, newPass))
                                {
                                    Console.WriteLine("Password Changed Successfully!");
                                }
                                else
                                {
                                    Console.WriteLine("Eror on change Password!");
                                }
                            }
                        }
                    }
                }
                else
                {
                    Console.WriteLine("Enter valid card number");
                }
            }
            catch (CardIsNotExistException ex)
            {
                Console.WriteLine(ex.Message);
            }
            catch (PasswordIncorrectException ex)
            {
                Console.WriteLine(ex.Message);
            }
            catch (CardIsNotActiveException ex)
            {
                Console.WriteLine(ex.Message);
            }
            catch (Exception ex)
            {
                Console.WriteLine(ex.Message);
            }
            Console.ReadKey();
            break;
        case 3:
            Console.WriteLine("Enter Card Number");
            string sourceCardNumber2 = Console.ReadLine();
            var transactions = _transactionService.GetTransactionByCardNumber(sourceCardNumber2);
            foreach (var t in transactions)
            {
                if (t.SourceCardNumber == sourceCardNumber2)
                {
                    Console.WriteLine($"Sent to: {t.DestinationCardNumber} {t.Amount} Status : {(t.IsSuccessfull?"Successfull":"UnSuccessfull")} at {t.TransactionDate} ");
                }
                else if (t.DestinationCardNumber == sourceCardNumber2)
                {
                    Console.WriteLine($"Recieve from: {t.SourceCardNumber} {t.Amount} Status : {(t.IsSuccessfull?"Successfull":"UnSuccessfull")} at {t.TransactionDate}");
                }
            }
            Console.ReadKey();
            break;
        case 0:
            outerFlag = false;
            break;
    }
} while (outerFlag);
