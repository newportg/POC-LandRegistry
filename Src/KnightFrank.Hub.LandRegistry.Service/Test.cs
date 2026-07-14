//using KnightFrank.Hub.LandRegistry.Common.Models;
//using System;
//using System.Collections.Generic;
//using System.Linq;
//using System.Text;
//using System.Threading.Tasks;

//namespace KnightFrank.Hub.LandRegistry.Service
//{
//    public class Client
//    {
//        public void Run()
//        {
//            var tsf = TestServiceFactory.GetServiceFactory("AE"); 
//        }
//    }
//    public abstract class TestServiceFactory
//    {
//        public static TestServiceFactory GetServiceFactory(string req)
//        {
//            if( req.Equals("AE"))
//            {
//                return new TestAEFactory();
//            }

//            return null;
//        }

//        public abstract TestLandRegistry GetService();
//    }

//    public class TestAEFactory : TestServiceFactory
//    {
//        public override TestLandRegistry GetService()
//        {
//            return new TestAE();
//        }
//    }

//    public abstract class TestLandRegistry
//    {
//        public abstract void Map();
//        public abstract void Validate();
//        public abstract void Request();
//        public abstract void Response();
//    }

//    public class TestAE : TestLandRegistry
//    {
//        public override void Map() { }
//        public override void Validate() { }
//        public override void Request() { }
//        public override void Response() { }
//    }
//}

