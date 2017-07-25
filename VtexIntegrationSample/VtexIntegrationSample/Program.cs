using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Enginesoft.VtexIntegrationSample
{
    public class Program
    {
        public static void Main(string[] args)
        {
            TestSendOrder();
        }

        private static void TestSendOrder()
        {
            var integration = VtexIntegration.Create();
            integration.LogEvent += Log;
            integration.LogServiceCallEvent += LogServiceCall;

            var integrationBusiness = new Business.IntegrationBusiness((int)supplierEnum);
            integrationBusiness.LogEvent += Log;
            integrationBusiness.LogServiceCallEvent += LogServiceCall;

            int clientID = 1253;
            int profileID = 0;
            string shippingTypeID = "SEDEX";
            int[] listItemIds = new int[] { 342911 };
            var paymentTypeID = Models.PaymentTypesEnum.Amex;
            
            var client = new Business.ClientBusiness().Get(clientID, true);
            var supplier = new Business.SupplierBusiness().Get((int)supplierEnum, true);
            var clientSupplier = client.ClientSuppliers.Where(a => a.SupplierID == (int)supplierEnum).First();

            client.Email = "sample@teste.com";

            var itemBusiness = new Business.ItemBusiness();
            var listItems = new List<Domain.Item.Item>();
            foreach (var id in listItemIds)
            {
                var item = itemBusiness.Get(id, profileID);
                if (item == null)
                    throw new System.InvalidOperationException("Invalid ItemID");

                listItems.Add(item);
            }

            var billingAddress = client.ClientAddresses.Where(a => a.ClientID == clientID && a.AddressTypeID == (int)Domain.Client.AddressTypeEnum.Billing).FirstOrDefault();
            if (billingAddress == null)
                throw new System.InvalidOperationException(string.Format("Client ID={0} não possui endereço de faturamento", clientID));

            var deliveryAddress = client.ClientAddresses.Where(a => a.ClientID == clientID && a.AddressTypeID == (int)Domain.Client.AddressTypeEnum.Delivery).FirstOrDefault();
            if (deliveryAddress == null)
                throw new System.InvalidOperationException(string.Format("Client ID={0} não possui endereço de entrega", clientID));

            var deliveryAddressIntegration = new Models.Address("Entrega", string.Format("{0} {1}", client.ContactFirstName, client.ContactLastName), deliveryAddress.Street, deliveryAddress.Number, deliveryAddress.Complement, deliveryAddress.Quarter, deliveryAddress.Zipcode, deliveryAddress.City, deliveryAddress.State, deliveryAddress.Country);

            var clientIntegration = GetClientIntegration(client, billingAddress);

            Dictionary<string, string> cookies = new Dictionary<string, string>();

            //---- simulation

            var responseGetItemsPrice = CheckCart((int)supplierEnum, clientIntegration, deliveryAddressIntegration, listItems);
            var jsonGetItemsPrice = Utils.JsonSerialize(responseGetItemsPrice);
            MergeDictionaries(cookies, responseGetItemsPrice.Cookies);

            //---- payment information

            var clientSupplierPaymentCondition = clientSupplier.ClientSupplierPaymentConditions.Where(a => a.SupplierPaymentCondition.PaymentConditionTypeID == (int)paymentTypeID).First();
            var supplierPaymentCondition = clientSupplierPaymentCondition.SupplierPaymentCondition;

            var paymentCondition = new Domain.Purchase.PaymentCondition(supplierPaymentCondition.SupplierPaymentConditionID, supplierPaymentCondition.SupplierPaymentConditionCode, supplierPaymentCondition.Name, supplierPaymentCondition.GroupName, paymentTypeID, 0, new List<Domain.Purchase.PaymentInstallment>());
            var paymentConditionInformation = new Domain.Purchase.PaymentConditionInformation(paymentCondition, 0, 1, 0);

            paymentConditionInformation.SetCreditCardInformation("376600000000000", "Jose da Silva", 2022, 12, "0000", "66186378489");
            //Ei você debugando, rode esse comando novamente no Immediate Window enviando dados válidos de cartão
            System.Diagnostics.Debugger.Break();

            //-------- create order request

            int itemQuantity = 1;
            int installmentQuantity = 1;

            var requestCloseOrderListItems = new List<Domain.Order.SendOrderRequestItem>();
            var requestCloseOrderIntegrationListItems = new List<Models.SendOrderRequestItem>();

            var listShippingInformations = new List<Models.ShippingInformation>();
            foreach (var itemPriceIntegration in responseGetItemsPrice.ItemsList)
            {
                var shippingInformationIntegration = itemPriceIntegration.ShippingInformations.Where(a => a.ShippingTypeID == shippingTypeID).FirstOrDefault();
                if (shippingInformationIntegration == null)
                    throw new System.InvalidOperationException("Condição de entrega não encontrada para o item");

                var shippingInformation = new Domain.Item.ShippingInformation(shippingTypeID, shippingTypeID, itemPriceIntegration.Value, new Domain.Item.ShippingTime(shippingInformationIntegration.ShippingTime.Days, shippingInformationIntegration.ShippingTime.IsBusinessDays));

                var item = listItems.Where(a => a.Barcode == itemPriceIntegration.Barcode).FirstOrDefault();

                listShippingInformations.Add(shippingInformationIntegration);

                requestCloseOrderListItems.Add(new Domain.Order.SendOrderRequestItem(item.ItemID, item.Name, item.Barcode, item.PackagesInBox, itemPriceIntegration.SupplierItemCode, 0, 0, itemQuantity, itemPriceIntegration.Value, itemQuantity * itemPriceIntegration.Value, itemPriceIntegration.SellerCode, shippingInformation));
                requestCloseOrderIntegrationListItems.Add(new Models.SendOrderRequestItem(itemPriceIntegration.Barcode, itemPriceIntegration.SupplierItemCode, itemPriceIntegration.SellerCode, itemQuantity, itemPriceIntegration.Value, itemQuantity * itemPriceIntegration.Value, shippingInformationIntegration));
            }

            int totalQuantity = requestCloseOrderIntegrationListItems.Sum(a => a.Quantity);
            decimal subTotalValue = requestCloseOrderIntegrationListItems.Sum(a => a.Total);
            decimal shippingValue = requestCloseOrderIntegrationListItems.Sum(a => a.ShippingInformation.Price);
            decimal orderTotalValue = subTotalValue + shippingValue;
            decimal installmentsValue = orderTotalValue / (decimal)installmentQuantity;

            paymentConditionInformation.SetValue(orderTotalValue);
            paymentConditionInformation.SetInstallmentValue(installmentQuantity, installmentsValue);

            var paymentConditionIntegration = new Models.PaymentCondition(paymentCondition.PaymentConditionCode, paymentCondition.Name, paymentCondition.GroupName, paymentTypeID, orderTotalValue, new List<Models.PaymentInstallment>());
            var paymentConditionInformationIntegration = new Models.PaymentConditionInformation(paymentConditionIntegration, paymentConditionInformation.Value, paymentConditionInformation.InstallmentsQuantity, paymentConditionInformation.InstallmentsValue);
            paymentConditionInformationIntegration.SetCreditCardInformation(paymentConditionInformation.CardNumber, paymentConditionInformation.HolderName, paymentConditionInformation.DueYear, paymentConditionInformation.DueMonth, paymentConditionInformation.ValidationCode, paymentConditionInformation.DocumentNumber);

            var listConsolidateShippingInformations = UtilsIntegration.ConsolidateShippingInformation(responseGetItemsPrice.ItemsList);
            var consolidateShippingInformationIntegration = listConsolidateShippingInformations.Where(a => a.ShippingTypeID == shippingTypeID).FirstOrDefault();
            var consolidateShippingInformation = new Domain.Item.ShippingInformation(consolidateShippingInformationIntegration.ShippingTypeID, consolidateShippingInformationIntegration.Name, consolidateShippingInformationIntegration.Price, new Domain.Item.ShippingTime(consolidateShippingInformationIntegration.ShippingTime.Days, consolidateShippingInformationIntegration.ShippingTime.IsBusinessDays));

            var requestCloserOrder = new Domain.Order.SendOrderRequest(client.ClientID, 1, (int)supplierEnum, 1, requestCloseOrderListItems, subTotalValue, orderTotalValue, totalQuantity, consolidateShippingInformation, deliveryAddress.ClientAddressID, paymentConditionInformation, cookies);
            var requestCloseOrderIntegration = new Models.SendOrderRequest(clientIntegration, orderTotalValue, requestCloseOrderIntegrationListItems, deliveryAddressIntegration, paymentConditionInformationIntegration, cookies);

            //----- send order
            System.Diagnostics.Debugger.Break();
            var responseCloseOrder = SendOrder((int)supplierEnum, requestCloseOrderIntegration);
            MergeDictionaries(cookies, requestCloseOrderIntegration.Coookies);

            //----- save order
            System.Diagnostics.Debugger.Break();
            var orderID = SaveOrder(client, billingAddress, deliveryAddress, requestCloserOrder, responseCloseOrder.OrderNumber, responseCloseOrder.BankSlipUrl, responseCloseOrder.PaymentTransactionCode, paymentConditionInformation);

            //---- send payment
            System.Diagnostics.Debugger.Break();
            var responseSendPayment = SendPayment((int)supplierEnum, clientIntegration, responseCloseOrder.OrderNumber, responseCloseOrder.PaymentTransactionCode, paymentConditionInformationIntegration);

            //------ complete order
            System.Diagnostics.Debugger.Break();
            var responseCompleteOrder = integrationBusiness.CompleteOrder(new Models.CompleteOrderRequest(clientIntegration, responseCloseOrder.OrderNumber, cookies));
            MergeDictionaries(cookies, requestCloseOrderIntegration.Coookies);

            //------ get payment information
            System.Diagnostics.Debugger.Break();
            var paymentStatus = GetPaymentStatus((int)supplierEnum, clientIntegration, responseCloseOrder.PaymentTransactionCode);

            System.Diagnostics.Debugger.Break();
        }

        private static void VtexCheckStatus()
        {
            var integrationBusiness = new Business.IntegrationBusiness((int)Models.SuppliersEnum.None);
            integrationBusiness.LogEvent += Log;
            integrationBusiness.LogServiceCallEvent += LogServiceCall;

            var cancelationToken = new System.Threading.CancellationTokenSource();
            integrationBusiness.VtexCheckStatus(cancelationToken);
        }

        private static Models.ClientIntegration GetClientIntegration(Domain.Client.Client client, Domain.Client.ClientAddress billingAddress)
        {
            var billingAddressIntegration = new Models.Address("Faturamento", string.Format("{0} {1}", client.ContactFirstName, client.ContactLastName), billingAddress.Street, billingAddress.Number, billingAddress.Complement, billingAddress.Quarter, billingAddress.Zipcode, billingAddress.City, billingAddress.State, billingAddress.Country);

            var clientIntegration = new Models.ClientIntegration(
                client.ClientID, client.CompanyName,
                client.ContactFirstName, client.ContactLastName, client.ContactCpf,
                client.Cnpj, client.Ie,
                client.Email, client.Phone, billingAddressIntegration);

            return clientIntegration;
        }

        private static Marketplace.Models.ServiceExecutionLog UpdateAllItems(int supplierID)
        {
            var integrationBusiness = new Business.IntegrationBusiness(supplierID);
            integrationBusiness.LogEvent += Log;
            integrationBusiness.LogServiceCallEvent += LogServiceCall;

            var cancelationTokenSource = new System.Threading.CancellationTokenSource();
            var log = integrationBusiness.VtexUpdateAllItems(cancelationTokenSource);
            return log;
        }

        private static Marketplace.Models.ServiceExecutionLog ProcessItemNotificatinon(int supplierID)
        {
            var integrationBusiness = new Business.IntegrationBusiness(supplierID);
            integrationBusiness.LogEvent += Log;
            integrationBusiness.LogServiceCallEvent += LogServiceCall;

            int successQuantity = 0;
            int errorQuantity = 0;
            Marketplace.Models.ServiceExecutionLog log = null;

            var cancelationTokenSource = new System.Threading.CancellationTokenSource();

            while (true)
            {
                log = integrationBusiness.ProcessVtexItemNotificationQueue(5, cancelationTokenSource);

                successQuantity += log.SuccessQuantity;
                errorQuantity += log.ErrorQuantity;

                if (log.SuccessQuantity == 0 && log.ErrorQuantity == 0)
                    break;
            }

            return log;
        }

        private static List<Domain.Purchase.PaymentConditionType> ListPaymentConditions(int supplierID)
        {
            var integrationBusiness = new Business.IntegrationBusiness(supplierID);
            integrationBusiness.LogEvent += Log;
            integrationBusiness.LogServiceCallEvent += LogServiceCall;

            var responsePaymentConditions = integrationBusiness.ListPaymentConditions();
            Console.WriteLine(string.Format("responsePaymentConditions -- response: {0}", Utils.JsonSerialize(responsePaymentConditions, true, true)));

            var paymentConditionTypes = new Business.PurchaseBusiness().ListPaymentConditionTypes();
            return paymentConditionTypes;
        }

        private static List<Models.GetItemResponse> ListItemsIntegration(int supplierID, List<Domain.Item.Item> listItems)
        {
            var integrationBusiness = new Business.IntegrationBusiness(supplierID);
            integrationBusiness.LogEvent += Log;
            integrationBusiness.LogServiceCallEvent += LogServiceCall;

            var integration = Integration.VtexIntegration.Create();
            integration.LogEvent += Log;
            integration.LogServiceCallEvent += LogServiceCall;

            var listResponse = new List<Models.GetItemResponse>();

            foreach (var item in listItems)
            {
                var supplierItem = item.SupplierItems.Where(a => a.SupplierID == supplierID).FirstOrDefault();

                var requestGet = new Models.GetItemRequest(supplierItem.SupplierItemCode);
                var responseGet = integration.GetItem(requestGet);
                Console.WriteLine(string.Format("responseItemGet: {0}", Utils.JsonSerialize(responseGet, true, true)));

                listResponse.Add(responseGet);
            }

            return listResponse;
        }

        private static Models.GetItemsPriceResponse CheckCart(int supplierID, Models.ClientIntegration clientIntegration, Models.Address deliveryAddressIntegration, List<Domain.Item.Item> listItem)
        {
            var integrationBusiness = new Business.IntegrationBusiness(supplierID);
            integrationBusiness.LogEvent += Log;
            integrationBusiness.LogServiceCallEvent += LogServiceCall;

            int itemQuantity = 1;

            var requestItemsPriceListItems = new List<Models.ItemGetItemsPriceRequest>();
            foreach (var item in listItem)
            {
                var supplierItem = item.SupplierItems.Where(a => a.SupplierID == supplierID).FirstOrDefault();
                if (supplierItem == null)
                    throw new System.InvalidOperationException("Invalid item");

                requestItemsPriceListItems.Add(new Models.ItemGetItemsPriceRequest(supplierItem.SupplierItemCode, item.Barcode, itemQuantity, item.PackagesInBox, supplierItem.SellerCode));
            }

            var requestItemsPrice = new Models.GetItemsPriceRequest(clientIntegration, deliveryAddressIntegration.Zipcode, requestItemsPriceListItems, null);
            var responseItemsPrice = integrationBusiness.GetItemsPrice(requestItemsPrice);

            return responseItemsPrice;
        }

        private static Models.SendOrderResponse SendOrder(int supplierID,
            Models.SendOrderRequest requestCloseOrderIntegration)
        {
            var integrationBusiness = new Business.IntegrationBusiness(supplierID);
            integrationBusiness.LogEvent += Log;
            integrationBusiness.LogServiceCallEvent += LogServiceCall;

            var responseCloseOrderIntegration = integrationBusiness.SendOrder(requestCloseOrderIntegration);

            if (responseCloseOrderIntegration.Status != Models.SendOrderStatusEnum.Success)
            {
                System.Diagnostics.Debugger.Break();
                throw new System.InvalidOperationException(string.Format("Não foi possível criar o pedido -- {0}", responseCloseOrderIntegration.Message));
            }

            Console.WriteLine(string.Format("responseCloseOrder -- status: {0}, response: {1}", responseCloseOrderIntegration.Status.ToString(), Utils.JsonSerialize(responseCloseOrderIntegration, true, true)));

            return responseCloseOrderIntegration;
        }

        private static int SaveOrder(Domain.Client.Client client,
            Domain.Client.ClientAddress billingAddress,
            Domain.Client.ClientAddress deliveryAddress,
            Domain.Order.SendOrderRequest requestCloseOrder,
            string orderNumber,
            string bankSlipUrl,
            string paymentTransactionCode,
            Domain.Purchase.PaymentConditionInformation paymentConditionInformation)
        {
            int orderID = 0;

            try
            {
                orderID = new Business.OrderBusiness().SaveOrderTest(requestCloseOrder, client, billingAddress, deliveryAddress, orderNumber, bankSlipUrl, paymentTransactionCode, paymentConditionInformation);
                Log(string.Format("OrderID={0}", orderID));
            }
            catch (System.Exception ex)
            {
                Console.WriteLine(string.Format("Erro ao salvar o pedido -- {0}", ex.Message));
            }

            return orderID;
        }

        private static Models.SendPaymentResponse SendPayment(int supplierID, Models.ClientIntegration clientIntegration, string supplierOrderNumber, string supplierTransactionCode, Models.PaymentConditionInformation paymentConditionInformation)
        {
            var integrationBusiness = new Business.IntegrationBusiness(supplierID);
            integrationBusiness.LogEvent += Log;
            integrationBusiness.LogServiceCallEvent += LogServiceCall;

            var request = new Models.SendPaymentRequest(clientIntegration, supplierOrderNumber, supplierTransactionCode, paymentConditionInformation);
            var response = integrationBusiness.SendPayment(request);
            return response;
        }

        private static Models.GetPaymentStatusResponse GetPaymentStatus(int supplierID, Models.ClientIntegration clientIntegration, string supplierTransactionCode)
        {
            var integrationBusiness = new Business.IntegrationBusiness(supplierID);
            integrationBusiness.LogEvent += Log;
            integrationBusiness.LogServiceCallEvent += LogServiceCall;

            var request = new Models.GetPaymentStatusRequest(clientIntegration, supplierTransactionCode);
            var response = integrationBusiness.GetPaymentStatus(request);
            return response;
        }

        #region Auxiliar Methods

        private static void Log(string message)
        {
            Console.WriteLine(Utils.MaxLength(message, 600, "..."));
            System.Diagnostics.Debug.WriteLine(message);

            string path = System.IO.Path.Combine(Utils.GetExecutableFolderPath(System.Reflection.Assembly.GetExecutingAssembly()), @"_log.txt");
            using (var streamWriter = new System.IO.StreamWriter(path, true))
            {
                streamWriter.WriteLine(string.Format("{0}\t{1}", DateTime.Now.ToString("yyyy-MM-dd HH:mm:ss"), message));
            }
        }

        private static void LogServiceCall(string supplierName, string message, int? clientID)
        {
            Console.WriteLine(Utils.MaxLength(message, 600, "..."));

            string path = System.IO.Path.Combine(Utils.GetExecutableFolderPath(System.Reflection.Assembly.GetExecutingAssembly()), @"_log.txt");
            using (var streamWriter = new System.IO.StreamWriter(path, true))
            {
                streamWriter.WriteLine(string.Format("{0}\t{1}", DateTime.Now.ToString("yyyy-MM-dd HH:mm:ss"), message));
            }
        }

        private static void MergeDictionaries(Dictionary<string, string> cookies, Dictionary<string, string> cookiesToAdd)
        {
            try
            {
                if (cookiesToAdd == null)
                    return;

                foreach (var item in cookiesToAdd)
                {
                    if (!cookies.ContainsKey(item.Key))
                        cookies.Add(item.Key, item.Value);
                    else
                        cookies[item.Key] = item.Value;
                }
            }
            catch (System.Exception ex)
            {
                Console.WriteLine(ex.Message);
            }
        }

        #endregion
    }
}
