using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using IntroSE.Kanban.Backend.ServiceLayer;

namespace IntroSE.Kanban.Frontend.Controllers
{
    internal class ControllerFactory
    {
        public static ControllerFactory Instance { get; } = new ControllerFactory();
        public readonly UserController UserController;
        public readonly BoardController BoardController;

        private ControllerFactory()
        {
            var serviceFactory = new ServiceFactory();
            this.UserController = new UserController(serviceFactory.US);
            this.BoardController = new BoardController(serviceFactory.BS);
        }
        // Might want to add TaskContoller ot send task service to board controller with board service
    }
}
