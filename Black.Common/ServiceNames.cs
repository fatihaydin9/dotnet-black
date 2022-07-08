using Black.Infrastructure.Attributes;
using System.Reflection;

namespace Black.Common;

public static class ServiceName
{
    public static List<string> GetServiceNames()
    {
        List<string> _serviceLst = new List<string>();
        string gatewayDirectory = Environment.CurrentDirectory;
        DirectoryInfo structureDirectory = Directory.GetParent(gatewayDirectory);
        string serviceDirectory = Path.Combine(structureDirectory.FullName, "Black.Service");
        List<string> folderNameLst = Directory.GetDirectories(serviceDirectory).Select(Path.GetFileName).ToList();

        foreach (var name in folderNameLst)
        {
            if (name.IndexOf("Service") > -1)
            {
                string service = name.Split("Service")[0];
                _serviceLst.Add(service.ToLower());
            }
        }

        return _serviceLst;
    }

    public static List<string> GetServiceNamesWithPostfix()
    {
        List<string> serviceLst = GetServiceNames();
        List<string> serviceLstPostfix = new List<string>();
        foreach (var service in serviceLst)
        {
            serviceLstPostfix.Add(service+"Service");
        }
        return serviceLstPostfix;
    }

    public static List<KeyValuePair<int, string>> GetServiceNamesWithIds()
    {
        List<string> serviceLst = GetServiceNames();
        List<KeyValuePair<int,string>> serviceLstIds = new List<KeyValuePair<int, string>>();
        int i = 1;
        foreach (var service in serviceLst)
        {
            serviceLstIds.Add(new KeyValuePair<int, string>(i, service));
            i++;
        }
        return serviceLstIds;
    }

    public static List<KeyValuePair<int, string>> GetServiceNamesWithPostfixAndIds()
    {
        List<string> serviceLst = GetServiceNames();
        List<KeyValuePair<int, string>> serviceLstIds = new List<KeyValuePair<int, string>>();
        int i = 1;
        foreach (var service in serviceLst)
        {
            serviceLstIds.Add(new KeyValuePair<int, string>(i, service+"Service"));
            i++;
        }
        return serviceLstIds;
    }

    public static List<ServiceDetail> GetServiceDetails()
    {
        List<ServiceDetail> serviceDetails = new List<ServiceDetail>();
        List<string> serviceLst = GetServiceNames();
        int i = 1;
        foreach (var service in serviceLst)
        {
            serviceDetails.Add(new ServiceDetail()
            {
                 Id = i,
                 Name= service,
                 NameWithPrefix= service + "Service"
            });
            i++;
        }
        return serviceDetails;
    }

}


