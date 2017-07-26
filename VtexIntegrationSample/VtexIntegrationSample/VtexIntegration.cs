using System;
using System.Collections.Generic;
using System.Diagnostics;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Enginesoft.VtexIntegrationSample
{
    public class VtexIntegration
    {
        //url da api commerce, ex: https://[install].vtexcommercestable.com.br
        private RestSharp.RestClient restSharpClientCommerce = null;

        //url da api payments, ex: https://[install].vtexpayments.com.br
        private RestSharp.RestClient restSharpClientPayment = null;

        private Models.IntegrationConfiguration Config;

        private VtexIntegration()
        {
            
        }

        public static VtexIntegration Create()
        {
            var integration = new VtexIntegration();
            integration.LoadIntegrationConfiguration();

            integration.restSharpClientCommerce = new RestSharp.RestClient(new Uri(integration.Config.CommerceApiUrl));
            integration.restSharpClientCommerce.Timeout = 60000;

            integration.restSharpClientPayment = new RestSharp.RestClient(new Uri(integration.Config.PaymentApiUrl));
            integration.restSharpClientPayment.Timeout = 60000;

            return integration;
        }

        private void LoadIntegrationConfiguration()
        {
            Models.IntegrationConfiguration config;

            string directoryPath = Utils.GetConfigDirectory();
            string path = System.IO.Path.Combine(directoryPath, "vtex.xml");
            
            if (!System.IO.File.Exists(path))
            {
                System.Diagnostics.Debugger.Break();
                //Arquivo de config não encontrado, será criado um arquivo "config.xml" modelo, preencha esse arquivo com os dados da sua conta de teste

                config = new Models.IntegrationConfiguration();
                config.CommerceApiUrl = "https://install.vtexcommercestable.com.br";
                config.PaymentApiUrl = "https://install.vtexpayments.com.br";
                config.ServiceKey = "teste@teste.com.br";
                config.ServiceToken = "DSFEWFEWFRRGEGREGER";
                config.OrderServiceKey = "vtexappkey-teste-ASASSS";
                config.OrderServiceToken = "FWFWEFWEFREGERGERGERGSSDSDFWWFEWFEWFEW";
                config.PartnerCode = "XXX";
                config.TradePolicyCode = "1";
                config.MerchantName = "install";

                using (var streamWriter = new System.IO.StreamWriter(path))
                {
                    new System.Xml.Serialization.XmlSerializer(typeof(Models.IntegrationConfiguration)).Serialize(streamWriter, config);
                }
            }
            
            try
            {
                using (var streamReader = new System.IO.StreamReader(path))
                {
                    var xmlSerializer = new System.Xml.Serialization.XmlSerializer(typeof(Models.IntegrationConfiguration));
                    config = (Models.IntegrationConfiguration)xmlSerializer.Deserialize(streamReader);
                }

                this.ValidateIntegrationConfiguration(config);
                this.Config = config;
            }
            catch (System.Exception ex)
            {
                throw new System.InvalidOperationException(string.Format("Erro na leitura do arquivo de configurações de integração -- path: {0} -- {1}", path, ex.Message), ex);
            }
        }

        private void ValidateIntegrationConfiguration(Models.IntegrationConfiguration config)
        {
            if (config == null)
                throw new System.InvalidOperationException("Configuração não carregada");

            if (string.IsNullOrEmpty(config.CommerceApiUrl))
                throw new System.InvalidOperationException("Chave \"ServiceUrl\" não preenchida");

            if (!Uri.IsWellFormedUriString(config.CommerceApiUrl, UriKind.Absolute))
                throw new System.InvalidOperationException("Chave \"ServiceUrl\" não é uma url válida");

            if (string.IsNullOrEmpty(config.ServiceKey))
                throw new System.InvalidOperationException("Chave \"ServiceKey\" não preenchida");

            if (string.IsNullOrEmpty(config.ServiceToken))
                throw new System.InvalidOperationException("Chave \"ServiceToken\" não preenchida");

            if (string.IsNullOrEmpty(config.OrderServiceKey))
                throw new System.InvalidOperationException("Chave \"OrderServiceKey\" não preenchida");

            if (string.IsNullOrEmpty(config.OrderServiceToken))
                throw new System.InvalidOperationException("Chave \"OrderServiceToken\" não preenchida");

            if (string.IsNullOrEmpty(config.PartnerCode))
                throw new System.InvalidOperationException("Chave \"PartnerCode\" não preenchido");

            if (string.IsNullOrEmpty(config.MerchantName))
                throw new System.InvalidOperationException("Chave \"MerchantName\" não preenchido");
        }
                
        public Models.GetOrderStatusResponse GetOrderStatus(Models.GetOrderStatusRequest request)
        {
            if (request == null)
                throw new ArgumentNullException(nameof(request));

            if (string.IsNullOrEmpty(request.SupplierOrderNumber))
                throw new System.InvalidOperationException(string.Format("Número do pedido não informado"));

            var restSharpClient = this.restSharpClientCommerce;
            string url = string.Format("api/oms/pvt/orders/{0}", request.SupplierOrderNumber);
            string fullUrl = string.Concat(restSharpClient.BaseUrl, url);

            var restSharpRequest = new RestSharp.RestRequest(url, RestSharp.Method.GET);
            restSharpRequest.AddHeader("Accept", "application/json");
            restSharpRequest.AddHeader("Content-Type", "application/json");
            restSharpRequest.AddHeader("X-VTEX-API-AppKey", this.Config.ServiceKey);
            restSharpRequest.AddHeader("X-VTEX-API-AppToken", this.Config.ServiceToken);

            string json = null;
            var maxAttempts = 3;
            Stopwatch stopwatch = null;

            RestSharp.IRestResponse restSharpResponse = null;
            ModelsVtex.GetOrderStatusResponse obj;
            string serviceCode = null;

            try
            {
                for (int attempt = 1; attempt <= maxAttempts; attempt++)
                {
                    try
                    {
                        this.LogServiceCall(string.Format("[{0}] req {1}", restSharpRequest.Method, fullUrl));

                        stopwatch = Stopwatch.StartNew();
                        restSharpResponse = restSharpClient.Execute(restSharpRequest);
                        stopwatch.Stop();

                        json = Utils.RemoveNonVisibleCharacters(restSharpResponse.Content);
                        this.LogServiceCall(string.Format("[{0}] ret {1} -- HttpStatusCode: {2} -- Response: {3} -- {4}ms", restSharpRequest.Method, fullUrl, (int)restSharpResponse.StatusCode, json, stopwatch.ElapsedMilliseconds.ToString("#,##0")));

                        if (restSharpResponse.StatusCode != System.Net.HttpStatusCode.OK)
                            throw new System.InvalidOperationException(string.Format("Retornado código HTTP: {0} {1}", (int)restSharpResponse.StatusCode, restSharpResponse.ErrorMessage));

                        serviceCode = ((int)restSharpResponse.StatusCode).ToString();

                        break;
                    }
                    catch (System.Exception ex)
                    {
                        if (attempt == maxAttempts)
                        {
                            System.Diagnostics.Debugger.Break();
                            throw ex;
                        }
                    }
                }

                obj = Utils.JsonDeserialize<ModelsVtex.GetOrderStatusResponse>(json);

                if (string.Equals(obj.status, "payment-approved", StringComparison.CurrentCultureIgnoreCase))
                    return Models.GetOrderStatusResponse.CreateSuccess(Models.OrderStatusEnum.Paid, null, null, null, null, null, null);
                else if (string.Equals(obj.status, "invoiced", StringComparison.CurrentCultureIgnoreCase))
                    return Models.GetOrderStatusResponse.CreateSuccess(Models.OrderStatusEnum.Billed, null, null, null, null, null, null);
                else if (string.Equals(obj.status, "canceled", StringComparison.CurrentCultureIgnoreCase))
                    return Models.GetOrderStatusResponse.CreateSuccess(Models.OrderStatusEnum.Canceled, null, null, null, null, null, null);
                else
                    throw new System.InvalidOperationException(string.Format("Status da transação do pedido é inválido (status:\"{0}\")", obj.status));
            }
            catch (System.Exception ex)
            {
                System.Diagnostics.Debugger.Break();
                this.LogServiceCall(string.Format("{0} EXCEPTION: {1} -- {2}ms", fullUrl, ex.Message, stopwatch.ElapsedMilliseconds.ToString("#,##0")));
                throw new System.InvalidOperationException(string.Format("Erro na integração -- método: {0} -- erro: {1}", fullUrl, ex.Message), ex);
            }
        }
        
        public Models.GetItemResponse GetItem(Models.GetItemRequest request)
        {
            var maxAttempts = 3;
            Models.Item item = null;

            if (request == null)
                throw new ArgumentNullException(nameof(request));

            if (string.IsNullOrEmpty(request.SupplierItemCode))
                throw new ArgumentNullException(nameof(request.SupplierItemCode));

            var restSharpClient = this.restSharpClientCommerce;
            string url = string.Format("api/catalog_system/pvt/sku/stockkeepingunitbyid/{0}", request.SupplierItemCode);
            string fullUrl = string.Concat(restSharpClient.BaseUrl, url);

            var restSharpRequest = new RestSharp.RestRequest(url, RestSharp.Method.GET);
            restSharpRequest.AddHeader("Accept", "application/json");
            restSharpRequest.AddHeader("Content-Type", "application/json");
            restSharpRequest.AddHeader("X-VTEX-API-AppKey", this.Config.ServiceKey);
            restSharpRequest.AddHeader("X-VTEX-API-AppToken", this.Config.ServiceToken);

            Stopwatch stopwatch = null;

            try
            {
                string json = null;
                RestSharp.IRestResponse restSharpResponse = null;

                string itemGroup = "";
                string itemCategory = "";
                string itemSubCategory = "";
                ModelsVtex.SkuInformationResponse skuInformation;

                for (int attempt = 1; attempt <= maxAttempts; attempt++)
                {
                    try
                    {
                        this.LogServiceCall(string.Format("[{0}] req {1} Request", restSharpRequest.Method, fullUrl));

                        stopwatch = Stopwatch.StartNew();
                        restSharpResponse = restSharpClient.Execute(restSharpRequest);
                        stopwatch.Stop();

                        json = Utils.RemoveNonVisibleCharacters(restSharpResponse.Content);
                        this.LogServiceCall(string.Format("[{0}] ret {1} -- HttpStatusCode: {2} -- Response: {3} -- {4}ms", restSharpRequest.Method, fullUrl, (int)restSharpResponse.StatusCode, json, stopwatch.ElapsedMilliseconds.ToString("#,##0")));

                        if (restSharpResponse.StatusCode != System.Net.HttpStatusCode.OK)
                            throw new System.InvalidOperationException(string.Format("Retornado código HTTP: {0} {1}", (int)restSharpResponse.StatusCode, restSharpResponse.ErrorMessage));

                        break;
                    }
                    catch (System.Exception ex)
                    {
                        if (attempt == maxAttempts)
                        {
                            System.Diagnostics.Debugger.Break();
                            throw ex;
                        }
                    }
                }

                skuInformation = Utils.JsonDeserialize<ModelsVtex.SkuInformationResponse>(json);

                GetCategoryNames(skuInformation.ProductCategories, out itemGroup, out itemCategory, out itemSubCategory);

                var seller = skuInformation.SkuSellers.Where(a => !string.Equals(a.SellerId, "1")).FirstOrDefault();
                string sellerCode = (seller != null ? seller.SellerId : null);

                string barcode = null;
                if (skuInformation.AlternateIds != null)
                {
                    if (!string.IsNullOrEmpty(skuInformation.AlternateIds.Ean))
                        barcode = skuInformation.AlternateIds.Ean;
                    else if (!string.IsNullOrEmpty(skuInformation.AlternateIds.RefId))
                        barcode = string.Concat("code_", Utils.RemoveSpecialCharacters(skuInformation.AlternateIds.RefId, true, true, true, true).ToLower());
                }

                if (barcode == null)
                    throw new System.InvalidOperationException(string.Format("Produto sku={0} não possui código de barras ou código alternativo", request.SupplierItemCode));

                item = new Models.Item(
                    barcode: barcode,
                    supplierItemCode: skuInformation.Id.ToString(),
                    name: skuInformation.NameComplete,
                    brand: skuInformation.BrandName,
                    urlImage: skuInformation.Images.Select(a => a.ImageUrl).FirstOrDefault(),
                    itemGroup: itemGroup,
                    itemCategory: itemCategory,
                    itemSubCategory: itemSubCategory,
                    description: skuInformation.ProductDescription,
                    sellerCode: sellerCode
                );

                item.Sanitize();
            }
            catch (System.Exception ex)
            {
                System.Diagnostics.Debugger.Break();
                this.LogServiceCall(string.Format("{0} EXCEPTION: {1} -- {2}ms", fullUrl, ex.Message, stopwatch.ElapsedMilliseconds.ToString("#,##0")));
                throw new System.InvalidOperationException(string.Format("Erro na integração -- método: {0} -- erro: {1}", fullUrl, ex.Message), ex);
            }

            return new Models.GetItemResponse() { Item = item };
        }

        private void GetCategoryNames(Dictionary<string, string> dicCategories, out string itemGroup, out string itemCategory, out string itemSubCategory)
        {
            itemGroup = "";
            itemCategory = "";
            itemSubCategory = "";

            var listCategories = dicCategories.Select(a => a.Value).Reverse().ToList();

            if (listCategories.Count >= 3)
            {
                // EPIs e Segurança > Calçados de Segurança > Botas de PVC

                itemGroup = listCategories[0];
                itemCategory = listCategories[1];
                itemSubCategory = listCategories[2];
            }
            else if (listCategories.Count == 2)
            {
                //Higiene e Limpeza > Tapetes

                itemGroup = "";
                itemCategory = listCategories[0];
                itemSubCategory = listCategories[1];
            }
            else if (listCategories.Count == 1)
            {
                itemGroup = "";
                itemCategory = "";
                itemSubCategory = listCategories[0];
            }
        }

        public Models.GetItemsPriceResponse GetItemsPrice(Models.GetItemsPriceRequest request)
        {
            if (request == null)
                throw new ArgumentNullException(nameof(request));

            if (request.ItemsList == null || request.ItemsList.Count == 0)
                throw new ArgumentNullException(nameof(request.ItemsList));

            var maxAttempts = 3;
            Stopwatch stopwatch = null;

            var restSharpClient = restSharpClientCommerce;
            string url = string.Format("api/checkout/pub/orderForms/simulation?sc={0}&affiliateId={1}", this.Config.TradePolicyCode, this.Config.PartnerCode);
            string fullUrl = string.Concat(restSharpClient.BaseUrl, url);

            var restSharpRequest = new RestSharp.RestRequest(url, RestSharp.Method.POST);
            restSharpRequest.AddHeader("Accept", "application/json");
            restSharpRequest.AddHeader("Content-Type", "application/json");
            restSharpRequest.AddHeader("X-VTEX-API-AppKey", this.Config.ServiceKey);
            restSharpRequest.AddHeader("X-VTEX-API-AppToken", this.Config.ServiceToken);

            var requestObj = new ModelsVtex.SkuPriceRequest()
            {
                postalCode = request.Zipcode,
                country = (!string.IsNullOrEmpty(request.Zipcode) ? "BRA" : null),
                items = request.ItemsList.Select(a => new ModelsVtex.SkuPriceRequest.Item()
                {
                    id = a.SupplierItemCode,
                    quantity = a.Quantity,
                    seller = (!string.IsNullOrEmpty(a.SellerCode) ? a.SellerCode : "1")
                }).ToList()
            };

            restSharpRequest.AddJsonBody(requestObj);

            string json = null;
            var cookies = new Dictionary<string, string>();

            var listItems = new List<Models.ItemPrice>();
            var listPaymentConditions = new List<Models.PaymentCondition>();

            RestSharp.IRestResponse restSharpResponse = null;
            ModelsVtex.SkuPriceResponse obj;

            try
            {
                for (int attempt = 1; attempt <= maxAttempts; attempt++)
                {
                    try
                    {
                        json = Utils.JsonSerialize(requestObj);

                        this.LogServiceCall(
                            string.Format("[{0}] req {1}\r\nHeaders: {2}\r\nCookies: {3}\r\nRequest: {4}\r\n", restSharpRequest.Method, fullUrl, GetRequestHeaders(restSharpRequest), GetRequestCookies(restSharpRequest), json));

                        stopwatch = Stopwatch.StartNew();
                        restSharpResponse = restSharpClient.Execute(restSharpRequest);
                        stopwatch.Stop();

                        cookies = new Dictionary<string, string>();
                        foreach (var cookie in restSharpResponse.Cookies)
                            cookies.Add(cookie.Name, cookie.Value);

                        json = Utils.RemoveNonVisibleCharacters(restSharpResponse.Content);

                        this.LogServiceCall(
                            string.Format("[{0}] ret {1} -- HttpStatusCode: {2}\r\nHeaders: {3}\r\nCookies: {4}\r\nResponse: {5}\r\n{6}ms\r\n", restSharpRequest.Method, fullUrl, (int)restSharpResponse.StatusCode, GetResponseHeaders(restSharpResponse), GetResponseCookies(restSharpResponse), json, stopwatch.ElapsedMilliseconds.ToString("#,##0")));

                        if (restSharpResponse.StatusCode != System.Net.HttpStatusCode.OK)
                            throw new System.InvalidOperationException(string.Format("Retornado código HTTP: {0} {1}", (int)restSharpResponse.StatusCode, restSharpResponse.ErrorMessage));

                        break;
                    }
                    catch (System.Exception ex)
                    {
                        if (attempt == maxAttempts)
                        {
                            System.Diagnostics.Debugger.Break();
                            throw ex;
                        }
                    }
                }

                obj = Utils.JsonDeserialize<ModelsVtex.SkuPriceResponse>(json);

                Models.ItemPrice itemPrice;
                for (int i = 0; i < obj.items.Count; i++)
                {
                    var nodeItem = obj.items[i];

                    var item = request.ItemsList.FirstOrDefault(a => string.Equals(a.SupplierItemCode, nodeItem.id));
                    if (item == null)
                        continue;

                    itemPrice = new Models.ItemPrice(
                        nodeItem.requestIndex,
                        i,
                        item.SupplierItemCode,
                        item.SellerCode,
                        item.ItemBarcode,
                        (decimal)nodeItem.price.GetValueOrDefault() / 100,
                        0,
                        0,
                        item.Quantity);

                    listItems.Add(itemPrice);
                }

                string shippingEstimate;
                Models.ShippingInformation shippingPrice;
                foreach (var nodeLogistics in obj.logisticsInfo)
                {
                    var item = listItems.FirstOrDefault(a => a.ItemIndex == nodeLogistics.itemIndex);
                    if (item == null)
                        throw new System.InvalidOperationException(string.Format("node \"logisticsInfo\" referencia um itemIndex={0} que não está na resposta", nodeLogistics.itemIndex));

                    item.SetStockInfo((item.Quantity <= nodeLogistics.stockBalance), nodeLogistics.stockBalance);

                    foreach (var nodeSla in nodeLogistics.slas)
                    {
                        shippingEstimate = nodeSla.shippingEstimate;

                        if (string.IsNullOrEmpty(shippingEstimate))
                            continue;

                        if (!shippingEstimate.EndsWith("bd") && !shippingEstimate.EndsWith("d"))
                            throw new System.InvalidOperationException(string.Format("\"shippingEstimate\" ({0}) inválido. Deve estar no padrão 7d (para dias) ou 7bd (para dias úteis)", shippingEstimate));

                        shippingPrice = new Models.ShippingInformation(
                            nodeSla.id,
                            nodeSla.name,
                            (decimal)nodeSla.price.GetValueOrDefault() / 100,
                            new Models.ShippingTime(Convert.ToInt32(Utils.GetNumericCharacters(shippingEstimate)), shippingEstimate.EndsWith("bd")));

                        item.ShippingInformations.Add(shippingPrice);
                    }
                }

                Models.PaymentCondition paymentCondition;
                List<Models.PaymentInstallment> paymentInstallmentList;
                foreach (var nodeInstallmentOption in obj.paymentData.installmentOptions)
                {
                    var paymentSystem = obj.paymentData.paymentSystems.FirstOrDefault(a => a.id == Convert.ToInt32(nodeInstallmentOption.paymentSystem));
                    if (paymentSystem == null)
                        throw new System.InvalidOperationException(string.Format("PaymentSystem={0} não encontrado no json da resposta", nodeInstallmentOption.paymentSystem));

                    paymentInstallmentList = new List<Models.PaymentInstallment>();
                    foreach (var nodeInstallment in nodeInstallmentOption.installments)
                        paymentInstallmentList.Add(new Models.PaymentInstallment(nodeInstallment.count, (decimal)nodeInstallment.value / 100, (decimal)nodeInstallment.total / 100));

                    paymentCondition = new Models.PaymentCondition(paymentSystem.id.ToString(), nodeInstallmentOption.paymentName, nodeInstallmentOption.paymentGroupName, Models.PaymentTypesEnum.None, (decimal)nodeInstallmentOption.value / 100, paymentInstallmentList);
                    listPaymentConditions.Add(paymentCondition);
                }
            }
            catch (System.Exception ex)
            {
                System.Diagnostics.Debugger.Break();
                this.LogServiceCall(string.Format("{0} EXCEPTION: {1} -- {2}ms", fullUrl, ex.Message, stopwatch.ElapsedMilliseconds.ToString("#,##0")));
                throw new System.InvalidOperationException(string.Format("Erro na integração -- método: {0} -- erro: {1}", fullUrl, ex.Message), ex);
            }

            var listConsolidateShippingInformations = Utils.ConsolidateShippingInformation(listItems);
            var response = new Models.GetItemsPriceResponse(Models.GetItemsPriceStatusEnum.Success, "OK", "200", listItems, listConsolidateShippingInformations, listPaymentConditions, cookies);
            return response;
        }

        private ModelsVtex.SendOrderRequest CreateSendOrderRequest(Models.SendOrderRequest request)
        {
            if (request == null)
                throw new ArgumentNullException(nameof(request));

            if (request.PaymentConditionInformation == null)
                throw new ArgumentNullException(nameof(request.PaymentConditionInformation));

            if (request.PaymentConditionInformation.PaymentCondition == null)
                throw new ArgumentNullException(nameof(request.PaymentConditionInformation.PaymentCondition));

            Models.SendOrderRequestItem item;

            var requestObj = new ModelsVtex.SendOrderRequest();

            requestObj.items = new List<ModelsVtex.SendOrderRequest.Item>();

            for (int i = 0; i < request.ItemsList.Count; i++)
            {
                item = request.ItemsList[i];

                var requestItem = new ModelsVtex.SendOrderRequest.Item();
                requestItem.id = item.SupplierItemCode;
                requestItem.quantity = item.Quantity;
                requestItem.seller = item.SellerCode;
                requestItem.price = Convert.ToInt32(item.Total * 100);
                requestItem.bundleItems = new List<object>();
                requestObj.items.Add(requestItem);
            }

            var billingAddress = request.ClientIntegration.BillingAddress;
            if (billingAddress == null)
                throw new System.InvalidOperationException("Endereço de faturamento não preenchido");

            var requestClient = new ModelsVtex.SendOrderRequest.ClientProfileData();
            requestClient.id = "clientProfileData";
            requestClient.email = request.ClientIntegration.Email;
            requestClient.firstName = request.ClientIntegration.ContactFirstName;
            requestClient.lastName = request.ClientIntegration.ContactLastName;
            requestClient.documentType = "cpf";
            requestClient.document = request.ClientIntegration.ContactCpf;
            requestClient.phone = request.ClientIntegration.Phone;
            requestClient.corporateName = request.ClientIntegration.CompanyName;
            requestClient.tradeName = request.ClientIntegration.CompanyName;
            requestClient.corporateDocument = request.ClientIntegration.Cnpj;
            requestClient.stateInscription = (!string.IsNullOrEmpty(request.ClientIntegration.Ie) ? request.ClientIntegration.Ie : "Isento");
            requestClient.corporatePhone = request.ClientIntegration.Phone;
            requestClient.isCorporate = true;
            requestClient.userProfileId = null;
            requestObj.clientProfileData = requestClient;

            var requestAddress = new ModelsVtex.SendOrderRequest.Address();
            requestAddress.addressType = request.DeliveryAddress.AddressType;
            requestAddress.receiverName = request.DeliveryAddress.ReceiverName;
            requestAddress.addressId = "1";
            requestAddress.postalCode = request.DeliveryAddress.Zipcode;
            requestAddress.city = request.DeliveryAddress.City;
            requestAddress.state = request.DeliveryAddress.State;
            requestAddress.country = "BRA";
            requestAddress.street = request.DeliveryAddress.Street;
            requestAddress.number = request.DeliveryAddress.Number;
            requestAddress.neighborhood = request.DeliveryAddress.Quarter;
            requestAddress.complement = request.DeliveryAddress.Complement;
            requestAddress.reference = null;
            requestAddress.geoCoordinates = new List<object>();

            var requestShippingData = new ModelsVtex.SendOrderRequest.ShippingData();
            requestShippingData.id = "shippingData";
            requestShippingData.address = requestAddress;
            requestObj.shippingData = requestShippingData;

            requestShippingData.logisticsInfo = new List<ModelsVtex.SendOrderRequest.LogisticsInfo>();
            for (int i = 0; i < request.ItemsList.Count; i++)
            {
                item = request.ItemsList[i];

                if (item.ShippingInformation == null)
                    throw new System.InvalidOperationException(string.Format("Item código de barras={0} não possui informação de entrega preenchida", item.ItemBarcode));

                var requestLogisticInfo = new ModelsVtex.SendOrderRequest.LogisticsInfo();
                requestLogisticInfo.itemIndex = i;
                requestLogisticInfo.selectedSla = item.ShippingInformation.Name;
                requestLogisticInfo.lockTTL = "8bd"; //período de reserva, obrigatório
                requestLogisticInfo.shippingEstimate = item.ShippingInformation.ShippingTime.ToStringVtex();
                requestLogisticInfo.price = Convert.ToInt32(item.ShippingInformation.Price * 100);
                requestLogisticInfo.deliveryWindow = null;

                requestShippingData.logisticsInfo.Add(requestLogisticInfo);
            }

            var requestPaymentData = new ModelsVtex.SendOrderRequest.PaymentData();
            requestPaymentData.payments = new List<ModelsVtex.SendOrderRequest.Payment>();
            var payment = new ModelsVtex.SendOrderRequest.Payment();
            payment.value = Convert.ToInt32(request.PaymentConditionInformation.Value * 100);
            payment.referenceValue = Convert.ToInt32(request.PaymentConditionInformation.Value * 100);
            payment.installments = 1;
            payment.paymentSystem = request.PaymentConditionInformation.PaymentCondition.PaymentConditionCode;
            requestPaymentData.payments.Add(payment);
            requestObj.paymentData = requestPaymentData;

            return requestObj;
        }

        public Models.SendOrderResponse SendOrder(Models.SendOrderRequest request)
        {
            if (request == null)
                throw new ArgumentNullException(nameof(request));

            if (request.PaymentConditionInformation == null)
                throw new ArgumentNullException(nameof(request.PaymentConditionInformation));

            if (request.PaymentConditionInformation.PaymentCondition == null)
                throw new ArgumentNullException(nameof(request.PaymentConditionInformation.PaymentCondition));

            var maxAttempts = 3;
            Stopwatch stopwatch = null;

            var restSharpClient = restSharpClientCommerce;
            string url = string.Format("api/checkout/pub/orders?sc={0}&affiliateId={1}", this.Config.TradePolicyCode, this.Config.PartnerCode);
            string fullUrl = string.Concat(restSharpClient.BaseUrl, url);

            var restSharpRequest = new RestSharp.RestRequest(url, RestSharp.Method.PUT);
            restSharpRequest.AddHeader("Content-Type", "application/json");
            restSharpRequest.AddHeader("Accept", "application/json");
            restSharpRequest.AddHeader("X-VTEX-API-AppKey", this.Config.OrderServiceKey);
            restSharpRequest.AddHeader("X-VTEX-API-AppToken", this.Config.OrderServiceToken);

            foreach (var cookie in request.Coookies)
                restSharpRequest.AddCookie(cookie.Key, cookie.Value);

            //monta o request
            var requestObj = CreateSendOrderRequest(request);

            restSharpRequest.AddJsonBody(requestObj);

            string serviceCode = null;
            string json = null;
            var cookies = new Dictionary<string, string>();

            ModelsVtex.SendOrderResponse responseObj = null;
            RestSharp.IRestResponse restSharpResponse = null;

            try
            {
                for (int attempt = 1; attempt <= maxAttempts; attempt++)
                {
                    try
                    {
                        json = Utils.JsonSerialize(requestObj);

                        this.LogServiceCall(
                            string.Format("[{0}] req {1}\r\nHeaders: {2}\r\nCookies: {3}\r\nRequest: {4}\r\n", restSharpRequest.Method, fullUrl, GetRequestHeaders(restSharpRequest), GetRequestCookies(restSharpRequest), json));

                        stopwatch = Stopwatch.StartNew();
                        restSharpResponse = restSharpClient.Execute(restSharpRequest);
                        stopwatch.Stop();

                        cookies = new Dictionary<string, string>();
                        foreach (var cookie in restSharpResponse.Cookies)
                            cookies.Add(cookie.Name, cookie.Value);

                        json = Utils.RemoveNonVisibleCharacters(restSharpResponse.Content);

                        this.LogServiceCall(
                            string.Format("[{0}] ret {1} -- HttpStatusCode: {2}\r\nHeaders: {3}\r\nCookies: {4}\r\nResponse: {5}\r\n{6}ms\r\n", restSharpRequest.Method, fullUrl, (int)restSharpResponse.StatusCode, GetResponseHeaders(restSharpResponse), GetResponseCookies(restSharpResponse), json, stopwatch.ElapsedMilliseconds.ToString("#,##0")));

                        serviceCode = ((int)restSharpResponse.StatusCode).ToString();

                        if (restSharpResponse.StatusCode == System.Net.HttpStatusCode.OK || restSharpResponse.StatusCode == System.Net.HttpStatusCode.Created)
                        {
                            //sucesso
                            responseObj = Utils.JsonDeserialize<ModelsVtex.SendOrderResponse>(json);
                        }
                        /*else if (restSharpResponse.StatusCode == System.Net.HttpStatusCode.BadRequest)
                        {
                            //foi retornado erro nos dados enviados
                            //PS. nesse caso retornou erro 400, mas deveria tentar novamente:
                            //HttpStatusCode: 400 -- Response: { "Fields": { }, "operationId": "a0f07ced-8a1a-4112-bba5-8c0faae31656", "error": { "code": "ORD003", "message": "Ocorreu um erro de comunicação com o Rates and Benefits", "exception": null } }
                            responseObj = JsonUtils.Deserialize<ModelsVtex.SendOrderResponse>(json);
                            return Models.CloseOrderResponse.CreateErrorResponse(Models.CloseOrderStatusEnum.Error, serviceCode, (responseObj != null && responseObj.orderForm != null && responseObj.orderForm.messages != null ? string.Join(", ", responseObj.orderForm.messages.Select(a => a.text)) : "Erro no envio do pedido"));
                        }*/
                        else
                        {
                            //erro crítico, possivelmente na comunicação
                            throw new System.InvalidOperationException(string.Format("Retornado código HTTP: {0} {1}", (int)restSharpResponse.StatusCode, restSharpResponse.ErrorMessage));
                        }

                        break;
                    }
                    catch (System.Exception ex)
                    {
                        if (attempt == maxAttempts)
                        {
                            System.Diagnostics.Debugger.Break();
                            throw ex;
                        }
                    }
                }

                if (responseObj.orders == null || responseObj.orders.Count == 0)
                    throw new System.InvalidOperationException("Erro ao processar a resposta do envio de pedido. Qtd pedidos na resposta=0");

                if (responseObj.transactionData == null)
                    throw new System.InvalidOperationException("Erro ao processar a resposta do envio de pedido. TransactionData=null");

                if (responseObj.transactionData.merchantTransactions == null || responseObj.transactionData.merchantTransactions.Count == 0)
                    throw new System.InvalidOperationException("Erro ao processar a resposta do envio de pedido. Qtd transactionData.merchantTransactions=0");

                string orderGroup = responseObj.orders.First().orderGroup;

                var merchantTransaction = responseObj.transactionData.merchantTransactions.FirstOrDefault();
                if (merchantTransaction == null)
                    throw new System.InvalidOperationException("Não foi retornado nenhum \"merchantTransactions\" na resposta do pedido");

                List<string> childOrders = responseObj.orders.Select(a => a.orderId).ToList();

                var response = Models.SendOrderResponse.CreateSuccessResponse(serviceCode, orderGroup, merchantTransaction.transactionId, null, childOrders, cookies);
                return response;
            }
            catch (System.Exception ex)
            {
                System.Diagnostics.Debugger.Break();
                this.LogServiceCall(string.Format("{0} EXCEPTION: {1} -- {2}ms", fullUrl, ex.Message, stopwatch.ElapsedMilliseconds.ToString("#,##0")));
                throw new System.InvalidOperationException(string.Format("Erro na integração -- método: {0} -- erro: {1}", fullUrl, ex.Message), ex);
            }
        }

        public Models.SendPaymentResponse SendPayment(Models.SendPaymentRequest request)
        {
            if (request == null)
                throw new ArgumentNullException(nameof(request));

            if (request.ClientIntegration == null)
                throw new ArgumentNullException(nameof(request.ClientIntegration));

            if (request.PaymentConditionInformation == null)
                throw new ArgumentNullException(nameof(request.PaymentConditionInformation));

            if (request.PaymentConditionInformation.PaymentCondition == null)
                throw new ArgumentNullException(nameof(request.PaymentConditionInformation.PaymentCondition));

            var maxAttempts = 3;
            Stopwatch stopwatch = null;

            if (string.IsNullOrEmpty(request.SupplierOrderNumber))
                throw new System.InvalidOperationException("Número do pedido não preenchido");

            var restSharpClient = this.restSharpClientPayment;
            string url = string.Format("split/{0}/payments", request.SupplierOrderNumber);
            string fullUrl = string.Concat(restSharpClient.BaseUrl, url);

            var restSharpRequest = new RestSharp.RestRequest(url, RestSharp.Method.POST);
            restSharpRequest.AddHeader("Content-Type", "application/json");
            restSharpRequest.AddHeader("Accept", "application/json");
            restSharpRequest.AddHeader("X-VTEX-API-AppKey", this.Config.OrderServiceKey);
            restSharpRequest.AddHeader("X-VTEX-API-AppToken", this.Config.OrderServiceToken);

            //--- payment
            var objPayment = new ModelsVtex.SendPaymentArrayItemRequest.Payment();
            objPayment.paymentSystem = Convert.ToInt32(request.PaymentConditionInformation.PaymentCondition.PaymentConditionCode);
            objPayment.paymentSystemName = request.PaymentConditionInformation.PaymentCondition.Name;
            objPayment.groupName = request.PaymentConditionInformation.PaymentCondition.GroupName;
            objPayment.currencyCode = null;
            objPayment.installments = request.PaymentConditionInformation.InstallmentQuantity;
            objPayment.value = Convert.ToInt32(request.PaymentConditionInformation.Value * 100);
            objPayment.installmentsInterestRate = 0;
            objPayment.installmentsValue = Convert.ToInt32(request.PaymentConditionInformation.InstallmentValue * 100);
            objPayment.referenceValue = Convert.ToInt32(request.PaymentConditionInformation.Value * 100);

            objPayment.fields = new ModelsVtex.SendPaymentArrayItemRequest.Fields();
            objPayment.fields.document = request.PaymentConditionInformation.DocumentNumber;
            objPayment.fields.accountId = null;
            objPayment.fields.addressId = null;
            objPayment.fields.cardNumber = request.PaymentConditionInformation.CardNumber;
            objPayment.fields.holderName = request.PaymentConditionInformation.HolderName;
            objPayment.fields.dueDate = request.PaymentConditionInformation.GetDueDateVtexTest();
            objPayment.fields.validationCode = request.PaymentConditionInformation.ValidationCode;

            objPayment.transaction = new ModelsVtex.SendPaymentArrayItemRequest.Transaction();
            objPayment.transaction.id = request.SupplierTransactionCode;
            objPayment.transaction.merchantName = this.Config.MerchantName;
            objPayment.transaction.payments = null;

            var arrayPayments = new ModelsVtex.SendPaymentArrayItemRequest.Payment[] { objPayment };

            var requestObj = new ModelsVtex.SendPaymentRequest();
            requestObj.paymentsArray = Utils.JsonSerialize(arrayPayments);

            //-------

            restSharpRequest.AddJsonBody(requestObj);

            string serviceCode = null;
            string json = null;
            var cookies = new Dictionary<string, string>();

            ModelsVtex.SendPaymentResponse responseObj = null;
            RestSharp.IRestResponse restSharpResponse = null;

            try
            {
                for (int attempt = 1; attempt <= maxAttempts; attempt++)
                {
                    try
                    {
                        json = Utils.JsonSerialize(requestObj);

                        //oculta dados sigilosos de cartão do log

                        if (!string.IsNullOrEmpty(objPayment.fields.cardNumber))
                            json = json.Replace(objPayment.fields.cardNumber, Utils.HideCreditCardNumber(objPayment.fields.cardNumber));

                        if (!string.IsNullOrEmpty(objPayment.fields.validationCode))
                            json = json.Replace(objPayment.fields.validationCode, Utils.HideCreditCardValidationCode(objPayment.fields.validationCode));

                        this.LogServiceCall(
                            string.Format("[{0}] req {1}\r\nHeaders: {2}\r\nCookies: {3}\r\nRequest: {4}\r\n", restSharpRequest.Method, fullUrl, GetRequestHeaders(restSharpRequest), GetRequestCookies(restSharpRequest), json));

                        stopwatch = Stopwatch.StartNew();
                        restSharpResponse = restSharpClient.Execute(restSharpRequest);
                        stopwatch.Stop();

                        cookies = new Dictionary<string, string>();
                        foreach (var cookie in restSharpResponse.Cookies)
                            cookies.Add(cookie.Name, cookie.Value);

                        json = Utils.RemoveNonVisibleCharacters(restSharpResponse.Content);

                        this.LogServiceCall(
                            string.Format("[{0}] ret {1} -- HttpStatusCode: {2}\r\nHeaders: {3}\r\nCookies: {4}\r\nResponse: {5}\r\n{6}ms\r\n", restSharpRequest.Method, fullUrl, (int)restSharpResponse.StatusCode, GetResponseHeaders(restSharpResponse), GetResponseCookies(restSharpResponse), json, stopwatch.ElapsedMilliseconds.ToString("#,##0")));

                        serviceCode = ((int)restSharpResponse.StatusCode).ToString();

                        if (restSharpResponse.StatusCode == System.Net.HttpStatusCode.OK || restSharpResponse.StatusCode == System.Net.HttpStatusCode.Created)
                        {
                            //sucesso
                            //responseObj = JsonUtils.Deserialize<ModelsVtex.SendPaymentResponse>(json);
                            return Models.SendPaymentResponse.CreateSuccessResponse();
                        }
                        else if (restSharpResponse.StatusCode == System.Net.HttpStatusCode.InternalServerError)
                        {
                            //foi retornado erro 500
                            return Models.SendPaymentResponse.CreateErrorResponse(Models.SendPaymentStatusEnum.Error, "Erro no envio do pagamento", serviceCode);
                        }
                        else if (restSharpResponse.StatusCode == System.Net.HttpStatusCode.BadRequest)
                        {
                            //foi retornado erro 400
                            responseObj = Utils.JsonDeserialize<ModelsVtex.SendPaymentResponse>(json);
                            return Models.SendPaymentResponse.CreateErrorResponse(Models.SendPaymentStatusEnum.Error, (responseObj != null && !string.IsNullOrEmpty(responseObj.message) ? responseObj.message : "Erro no envio do pagamento"), serviceCode);
                        }
                        else
                        {
                            //erro crítico, possivelmente na comunicação
                            throw new System.InvalidOperationException(string.Format("Retornado código HTTP: {0} {1}", (int)restSharpResponse.StatusCode, restSharpResponse.ErrorMessage));
                        }
                    }
                    catch (System.Exception ex)
                    {
                        if (attempt == maxAttempts)
                        {
                            System.Diagnostics.Debugger.Break();
                            throw ex;
                        }
                    }
                }

                throw new System.InvalidOperationException("Excedeu número de tentativas");
            }
            catch (System.Exception ex)
            {
                System.Diagnostics.Debugger.Break();
                this.LogServiceCall(string.Format("{0} EXCEPTION: {1} -- {2}ms", fullUrl, ex.Message, stopwatch.ElapsedMilliseconds.ToString("#,##0")));
                throw new System.InvalidOperationException(string.Format("Erro na integração -- método: {0} -- erro: {1}", fullUrl, ex.Message), ex);
            }
        }

        public Models.CompleteOrderResponse CompleteOrder(Models.CompleteOrderRequest request)
        {
            if (request == null)
                throw new ArgumentNullException(nameof(request));

            if (string.IsNullOrEmpty(request.SupplierOrderNumber))
                throw new System.InvalidOperationException(string.Format("Número do pedido do fornecedor não informado"));

            var maxAttempts = 3;
            Stopwatch stopwatch = null;

            var restSharpClient = this.restSharpClientCommerce;
            string url = string.Format("checkout/gatewayCallback/{0}/success", request.SupplierOrderNumber);
            string fullUrl = string.Concat(restSharpClient.BaseUrl, url);

            var restSharpRequest = new RestSharp.RestRequest(url, RestSharp.Method.GET);
            restSharpRequest.AddHeader("Accept", "application/json");
            restSharpRequest.AddHeader("Content-Type", "application/json");
            restSharpRequest.AddHeader("X-VTEX-API-AppKey", this.Config.OrderServiceKey);
            restSharpRequest.AddHeader("X-VTEX-API-AppToken", this.Config.OrderServiceToken);

            foreach (var cookie in request.Cookies)
                restSharpRequest.AddCookie(cookie.Key, cookie.Value);

            object requestObj = null;

            string serviceCode = null;
            string json = null;
            var cookies = new Dictionary<string, string>();

            var listItems = new List<Models.ItemPrice>();
            var listPaymentConditions = new List<Models.PaymentCondition>();

            RestSharp.IRestResponse restSharpResponse = null;

            try
            {
                for (int attempt = 1; attempt <= maxAttempts; attempt++)
                {
                    try
                    {
                        json = Utils.JsonSerialize(requestObj);

                        this.LogServiceCall(
                            string.Format("[{0}] req {1}\r\nHeaders: {2}\r\nCookies: {3}\r\nRequest: {4}\r\n", restSharpRequest.Method, fullUrl, GetRequestHeaders(restSharpRequest), GetRequestCookies(restSharpRequest), json));

                        stopwatch = Stopwatch.StartNew();
                        restSharpResponse = restSharpClient.Execute(restSharpRequest);
                        stopwatch.Stop();

                        cookies = new Dictionary<string, string>();
                        foreach (var cookie in restSharpResponse.Cookies)
                            cookies.Add(cookie.Name, cookie.Value);

                        json = Utils.RemoveNonVisibleCharacters(restSharpResponse.Content);
                        if (!string.IsNullOrEmpty(json) && json.IndexOf("<html", StringComparison.CurrentCultureIgnoreCase) != -1)
                            json = Utils.MaxLength(json, 1000, "...");

                        this.LogServiceCall(
                            string.Format("[{0}] ret {1}\r\nHttpStatusCode: {2}\r\nHeaders: {3}\r\nCookies: {4}\r\nResponse: {5}\r\n{6}ms\r\n", restSharpRequest.Method, fullUrl, (int)restSharpResponse.StatusCode, GetResponseHeaders(restSharpResponse), GetResponseCookies(restSharpResponse), json, stopwatch.ElapsedMilliseconds.ToString("#,##0")));

                        if (restSharpResponse.StatusCode != System.Net.HttpStatusCode.OK)
                            throw new System.InvalidOperationException(string.Format("Retornado código HTTP: {0} {1}", (int)restSharpResponse.StatusCode, restSharpResponse.ErrorMessage));

                        serviceCode = ((int)restSharpResponse.StatusCode).ToString();

                        break;
                    }
                    catch (System.Exception ex)
                    {
                        if (attempt == maxAttempts)
                        {
                            System.Diagnostics.Debugger.Break();
                            throw ex;
                        }
                    }
                }

                var ret = Models.CompleteOrderResponse.CreateSuccessResponse(serviceCode);
                return ret;
            }
            catch (System.Exception ex)
            {
                System.Diagnostics.Debugger.Break();
                this.LogServiceCall(string.Format("{0} EXCEPTION: {1} -- {2}ms", fullUrl, ex.Message, stopwatch.ElapsedMilliseconds.ToString("#,##0")));
                throw new System.InvalidOperationException(string.Format("Erro na integração -- método: {0} -- erro: {1}", fullUrl, ex.Message), ex);
            }
        }

        public Models.ListPaymentConditionsResponse ListPaymentConditions()
        {
            var maxAttempts = 3;

            var restSharpClient = this.restSharpClientPayment;
            string url = string.Format("api/pvt/merchants/payment-systems");
            string fullUrl = string.Concat(restSharpClient.BaseUrl, url);

            var restSharpRequest = new RestSharp.RestRequest(url, RestSharp.Method.GET);
            restSharpRequest.AddHeader("Content-Type", "application/json");
            restSharpRequest.AddHeader("X-VTEX-API-AppKey", this.Config.ServiceKey);
            restSharpRequest.AddHeader("X-VTEX-API-AppToken", this.Config.ServiceToken);

            Stopwatch stopwatch = null;

            try
            {
                string json = null;
                RestSharp.IRestResponse restSharpResponse = null;

                for (int attempt = 1; attempt <= maxAttempts; attempt++)
                {
                    try
                    {
                        this.LogServiceCall(string.Format("[{0}] req {1}", restSharpRequest.Method, fullUrl));

                        stopwatch = Stopwatch.StartNew();
                        restSharpResponse = restSharpClient.Execute(restSharpRequest);
                        stopwatch.Stop();

                        json = Utils.RemoveNonVisibleCharacters(restSharpResponse.Content);
                        this.LogServiceCall(string.Format("[{0}] ret {1} -- HttpStatusCode: {2} -- Response: {4} -- {4}ms", restSharpRequest.Method, fullUrl, (int)restSharpResponse.StatusCode, json, stopwatch.ElapsedMilliseconds.ToString("#,##0")));

                        if (restSharpResponse.StatusCode != System.Net.HttpStatusCode.OK)
                            throw new System.InvalidOperationException(string.Format("Retornado código HTTP: {0} {1}", (int)restSharpResponse.StatusCode, restSharpResponse.ErrorMessage));

                        break;
                    }
                    catch (System.Exception ex)
                    {
                        if (attempt == maxAttempts)
                        {
                            System.Diagnostics.Debugger.Break();
                            throw ex;
                        }
                    }
                }

                var list = Utils.JsonDeserialize<List<ModelsVtex.PaymentConditionResponse>>(json);

                var listRet = list.Select(a => new Models.PaymentCondition(a.id.ToString(), a.name, a.groupName, Utils.GetPaymentConditionTypeID(a.implementation), 0, null)).ToList();

                var response = new Models.ListPaymentConditionsResponse(listRet);
                return response;
            }
            catch (System.Exception ex)
            {
                System.Diagnostics.Debugger.Break();
                this.LogServiceCall(string.Format("{0} EXCEPTION: {1} -- {2}ms", fullUrl, ex.Message, stopwatch.ElapsedMilliseconds.ToString("#,##0")));
                throw new System.InvalidOperationException(string.Format("Erro na integração -- método: {0} -- erro: {1}", fullUrl, ex.Message), ex);
            }
        }

        public Models.GetPaymentStatusResponse GetPaymentStatus(Models.GetPaymentStatusRequest request)
        {
            if (request == null)
                throw new ArgumentNullException(nameof(request));

            if (string.IsNullOrEmpty(request.SupplierTransactionCode))
                throw new System.InvalidOperationException(string.Format("Código da transação não informado"));

            var restSharpClient = this.restSharpClientPayment;
            string url = string.Format("api/pvt/transactions/{0}", request.SupplierTransactionCode);
            string fullUrl = string.Concat(restSharpClient.BaseUrl, url);

            var restSharpRequest = new RestSharp.RestRequest(url, RestSharp.Method.GET);
            restSharpRequest.AddHeader("Accept", "application/json");
            restSharpRequest.AddHeader("Content-Type", "application/json");
            restSharpRequest.AddHeader("X-VTEX-API-AppKey", this.Config.ServiceKey);
            restSharpRequest.AddHeader("X-VTEX-API-AppToken", this.Config.ServiceToken);

            string json = null;
            var maxAttempts = 3;
            Stopwatch stopwatch = null;

            var listItems = new List<Models.ItemPrice>();
            var listPaymentConditions = new List<Models.PaymentCondition>();

            RestSharp.IRestResponse restSharpResponse = null;
            ModelsVtex.GetPaymentStatusResponse obj;
            string serviceCode = null;

            try
            {
                for (int attempt = 1; attempt <= maxAttempts; attempt++)
                {
                    try
                    {
                        this.LogServiceCall(string.Format("[{0}] req {1}", restSharpRequest.Method, fullUrl));

                        stopwatch = Stopwatch.StartNew();
                        restSharpResponse = restSharpClient.Execute(restSharpRequest);
                        stopwatch.Stop();

                        json = Utils.RemoveNonVisibleCharacters(restSharpResponse.Content);
                        this.LogServiceCall(string.Format("[{0}] ret {1} -- HttpStatusCode: {2} -- Response: {3} -- {4}ms", restSharpRequest.Method, fullUrl, (int)restSharpResponse.StatusCode, json, stopwatch.ElapsedMilliseconds.ToString("#,##0")));

                        if (restSharpResponse.StatusCode != System.Net.HttpStatusCode.OK)
                            throw new System.InvalidOperationException(string.Format("Retornado código HTTP: {0} {1}", (int)restSharpResponse.StatusCode, restSharpResponse.ErrorMessage));

                        serviceCode = ((int)restSharpResponse.StatusCode).ToString();

                        break;
                    }
                    catch (System.Exception ex)
                    {
                        if (attempt == maxAttempts)
                        {
                            System.Diagnostics.Debugger.Break();
                            throw ex;
                        }
                    }
                }

                obj = Utils.JsonDeserialize<ModelsVtex.GetPaymentStatusResponse>(json);

                var ret = new Models.GetPaymentStatusResponse();
                ret.Status = Models.GetPaymentStatusEnum.Success;
                ret.Message = "OK";
                ret.ServiceCode = serviceCode;

                if (string.Equals(obj.status, "Started", StringComparison.CurrentCultureIgnoreCase))
                    ret.PaymentStatus = Models.PaymentStatusEnum.Pendent;
                else if (string.Equals(obj.status, "Cancelled", StringComparison.CurrentCultureIgnoreCase))
                    ret.PaymentStatus = Models.PaymentStatusEnum.Cancelled;
                else
                    throw new System.InvalidOperationException(string.Format("Status da transação de pagamento é inválido (status:\"{0}\")", obj.status));

                return ret;
            }
            catch (System.Exception ex)
            {
                System.Diagnostics.Debugger.Break();
                this.LogServiceCall(string.Format("{0} EXCEPTION: {1} -- {2}ms", fullUrl, ex.Message, stopwatch.ElapsedMilliseconds.ToString("#,##0")));
                throw new System.InvalidOperationException(string.Format("Erro na integração -- método: {0} -- erro: {1}", fullUrl, ex.Message), ex);
            }
        }

        private string GetCookiesText(Dictionary<string, string> cookies)
        {
            if (cookies == null)
                return null;

            string text = string.Join(",", cookies.Select(a => string.Format("{0}={1}", a.Key, a.Value)));
            return text;
        }

        private static string GetRequestHeaders(RestSharp.IRestRequest request)
        {
            if (request == null)
                return null;

            string text = string.Join(", ", request.Parameters.Where(a => a.Type == RestSharp.ParameterType.HttpHeader).Select(a => string.Format("{0}={1}", a.Name, a.Value)));
            return text;
        }

        private static string GetRequestCookies(RestSharp.IRestRequest request)
        {
            if (request == null)
                return null;

            string text = string.Join(", ", request.Parameters.Where(a => a.Type == RestSharp.ParameterType.Cookie).Select(a => string.Format("{0}={1}", a.Name, a.Value)));
            return text;
        }

        private static string GetResponseHeaders(RestSharp.IRestResponse response)
        {
            if (response == null)
                return null;

            string text = string.Join(", ", response.Headers.Select(a => string.Format("{0}={1}", a.Name, a.Value)));
            return text;
        }

        private static string GetResponseCookies(RestSharp.IRestResponse response)
        {
            if (response == null)
                return null;

            string text = string.Join(", ", response.Cookies.Select(a => string.Format("{0}={1}", a.Name, a.Value)));
            return text;
        }

        #region LogEvent

        public delegate void LogEventHandler(string message);

        [System.Diagnostics.CodeAnalysis.SuppressMessage("Microsoft.Design", "CA1009:DeclareEventHandlersCorrectly")]
        public event LogEventHandler LogEvent;

        protected void Log(string message)
        {
            if (this.LogEvent != null)
                this.LogEvent(message);
        }

        #endregion

        #region LogServiceCall

        public delegate void LogServiceCallEventHandler(string message);

        [System.Diagnostics.CodeAnalysis.SuppressMessage("Microsoft.Design", "CA1009:DeclareEventHandlersCorrectly")]
        public event LogServiceCallEventHandler LogServiceCallEvent;

        protected void LogServiceCall(string message)
        {
            System.Diagnostics.Debug.WriteLine(message);

            if (this.LogServiceCallEvent != null)
                this.LogServiceCallEvent(message);
        }

        #endregion
    }
}
