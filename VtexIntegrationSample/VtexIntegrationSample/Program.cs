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

            Console.WriteLine();
            Console.WriteLine("Bye!");
            Console.ReadKey();
        }

        private static void TestSendOrder()
        {
            var integration = VtexIntegration.Create();
            integration.LogEvent += Log;
            integration.LogServiceCallEvent += LogServiceCall;
            
            //Dados para teste
            string shippingTypeID = "SEDEX";
            int[] listItemIds = new int[] { 342911 };
            var paymentTypeID = Models.PaymentTypesEnum.Amex;
            
            //Itens para enviar no pedido
            var listItems = new List<Models.Item>();
            listItems.Add(new Models.Item("988", "7894854031548", "Máscara TNT Tipo Calcinha Branca SKY", "SKY DESCARTÁVEIS", "", "CASA E CONSTRUÇÃO", "Limpeza", "Sabão / Detergente para Roupa", "", "protcap"));
            
            //Dados do cliente
            var clientIntegration = GetClientIntegration();
            var deliveryAddressIntegration = GetClientDeliveryAddress();
            
            Dictionary<string, string> cookies = new Dictionary<string, string>();

            //---- simulation

            var responseGetItemsPrice = CheckCart(integration, clientIntegration, deliveryAddressIntegration, listItems);
            var jsonGetItemsPrice = Utils.JsonSerialize(responseGetItemsPrice);
            Utils.MergeDictionaries(cookies, responseGetItemsPrice.Cookies);
            System.Diagnostics.Debugger.Break();

            /*
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

            var requestCloseOrderListItems = new List<Models.SendOrderRequestItem>();
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
            var responseCloseOrder = SendOrder(integration, requestCloseOrderIntegration);
            Utils.MergeDictionaries(cookies, requestCloseOrderIntegration.Coookies);

            //----- save order
            //TODO: Salvar

            //---- send payment
            System.Diagnostics.Debugger.Break();
            var responseSendPayment = SendPayment(integration, clientIntegration, responseCloseOrder.OrderNumber, responseCloseOrder.PaymentTransactionCode, paymentConditionInformationIntegration);

            //------ complete order
            System.Diagnostics.Debugger.Break();
            var responseCompleteOrder = integrationBusiness.CompleteOrder(integration, new Models.CompleteOrderRequest(clientIntegration, responseCloseOrder.OrderNumber, cookies));
            Utils.MergeDictionaries(cookies, requestCloseOrderIntegration.Coookies);

            //------ get payment information
            System.Diagnostics.Debugger.Break();
            var paymentStatus = GetPaymentStatus(integration, clientIntegration, responseCloseOrder.PaymentTransactionCode);
            */

            System.Diagnostics.Debugger.Break();
        }
        
        private static Models.ClientIntegration GetClientIntegration()
        {
            string directoryPath = Utils.GetConfigDirectory();
            string path = System.IO.Path.Combine(directoryPath, "client.xml");
            
            Models.ClientIntegration client;

            if (!System.IO.File.Exists(path))
            {
                var billingAddress = GetClientBillingAddress();
                client = new Models.ClientIntegration(0, "Empresa Teste", "Jose", "Teste", "14452872182", "96379179000109", "Isento", "teste@teste.com.br", "11 6666-6666", billingAddress);

                using (var streamWriter = new System.IO.StreamWriter(path))
                {
                    new System.Xml.Serialization.XmlSerializer(typeof(Models.ClientIntegration)).Serialize(streamWriter, client);
                }
            }

            try
            {
                using (var streamReader = new System.IO.StreamReader(path))
                {
                    var xmlSerializer = new System.Xml.Serialization.XmlSerializer(typeof(Models.ClientIntegration));
                    client = (Models.ClientIntegration)xmlSerializer.Deserialize(streamReader);
                }
            }
            catch (System.Exception ex)
            {
                throw new System.InvalidOperationException(string.Format("Erro na leitura do arquivo com os dados do cliente -- path: {0} -- {1}", path, ex.Message), ex);
            }

            return client;
        }

        private static Models.Address GetClientBillingAddress()
        {
            string directoryPath = Utils.GetConfigDirectory();
            string path = System.IO.Path.Combine(directoryPath, "clientBillingAddress.xml");

            Models.Address address;
            if (!System.IO.File.Exists(path))
            {
                address = new Models.Address("principal", "Jose Teste", "Av Paulista", "1000", "cj 77", "Cerqueira César", "01311200", "São Paulo", "SP", "BRA");

                using (var streamWriter = new System.IO.StreamWriter(path))
                {
                    new System.Xml.Serialization.XmlSerializer(typeof(Models.Address)).Serialize(streamWriter, address);
                }
            }

            try
            {
                using (var streamReader = new System.IO.StreamReader(path))
                {
                    var xmlSerializer = new System.Xml.Serialization.XmlSerializer(typeof(Models.Address));
                    address = (Models.Address)xmlSerializer.Deserialize(streamReader);
                }
            }
            catch (System.Exception ex)
            {
                throw new System.InvalidOperationException(string.Format("Erro na leitura do arquivo com o endereço de faturamento -- path: {0} -- {1}", path, ex.Message), ex);
            }

            return address;
        }

        private static Models.Address GetClientDeliveryAddress()
        {
            string directoryPath = Utils.GetConfigDirectory();
            string path = System.IO.Path.Combine(directoryPath, "clientDeliveryAddress.xml");

            Models.Address address;
            if (!System.IO.File.Exists(path))
            {
                address = new Models.Address("entrega", "Jose Teste", "Av Paulista", "1000", "cj 77", "Cerqueira César", "01311200", "São Paulo", "SP", "BRA");

                using (var streamWriter = new System.IO.StreamWriter(path))
                {
                    new System.Xml.Serialization.XmlSerializer(typeof(Models.Address)).Serialize(streamWriter, address);
                }
            }

            try
            {
                using (var streamReader = new System.IO.StreamReader(path))
                {
                    var xmlSerializer = new System.Xml.Serialization.XmlSerializer(typeof(Models.Address));
                    address = (Models.Address)xmlSerializer.Deserialize(streamReader);
                }
            }
            catch (System.Exception ex)
            {
                throw new System.InvalidOperationException(string.Format("Erro na leitura do arquivo com o endereço de faturamento -- path: {0} -- {1}", path, ex.Message), ex);
            }

            return address;
        }
        
        private static Models.GetItemsPriceResponse CheckCart(VtexIntegration integration, Models.ClientIntegration clientIntegration, Models.Address deliveryAddressIntegration, List<Models.Item> listItem)
        {
            int itemQuantity = 1;

            var requestItemsPriceListItems = new List<Models.ItemGetItemsPriceRequest>();
            foreach (var item in listItem)
                requestItemsPriceListItems.Add(new Models.ItemGetItemsPriceRequest(item.SupplierItemCode, item.Barcode, itemQuantity, item.SellerCode));

            var requestItemsPrice = new Models.GetItemsPriceRequest(clientIntegration, deliveryAddressIntegration.Zipcode, requestItemsPriceListItems, null);
            var responseItemsPrice = integration.GetItemsPrice(requestItemsPrice);

            return responseItemsPrice;
        }

        private static Models.SendOrderResponse SendOrder(VtexIntegration integration, Models.SendOrderRequest requestCloseOrderIntegration)
        {
            var responseCloseOrderIntegration = integration.SendOrder(requestCloseOrderIntegration);

            if (responseCloseOrderIntegration.Status != Models.SendOrderStatusEnum.Success)
            {
                System.Diagnostics.Debugger.Break();
                throw new System.InvalidOperationException(string.Format("Não foi possível criar o pedido -- {0}", responseCloseOrderIntegration.Message));
            }

            Console.WriteLine(string.Format("responseCloseOrder -- status: {0}, response: {1}", responseCloseOrderIntegration.Status.ToString(), Utils.JsonSerialize(responseCloseOrderIntegration, true, true)));

            return responseCloseOrderIntegration;
        }
        
        private static Models.SendPaymentResponse SendPayment(VtexIntegration integration, Models.ClientIntegration clientIntegration, string supplierOrderNumber, string supplierTransactionCode, Models.PaymentConditionInformation paymentConditionInformation)
        {
            var request = new Models.SendPaymentRequest(clientIntegration, supplierOrderNumber, supplierTransactionCode, paymentConditionInformation);
            var response = integration.SendPayment(request);
            return response;
        }

        private static Models.GetPaymentStatusResponse GetPaymentStatus(VtexIntegration integration, Models.ClientIntegration clientIntegration, string supplierTransactionCode)
        {
            var request = new Models.GetPaymentStatusRequest(clientIntegration, supplierTransactionCode);
            var response = integration.GetPaymentStatus(request);
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

        private static void LogServiceCall(string message)
        {
            Console.WriteLine(Utils.MaxLength(message, 600, "..."));

            string path = System.IO.Path.Combine(Utils.GetExecutableFolderPath(System.Reflection.Assembly.GetExecutingAssembly()), @"_log.txt");
            using (var streamWriter = new System.IO.StreamWriter(path, true))
            {
                streamWriter.WriteLine(string.Format("{0}\t{1}", DateTime.Now.ToString("yyyy-MM-dd HH:mm:ss"), message));
            }
        }
        
        #endregion
    }
}
