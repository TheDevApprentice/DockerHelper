using Installer.Program.UserInputs;
using System.Text;

namespace Installer.Packages.Docker
{
    public class DockerCompose
    {
        private string dockerPath;

        public string DockerPath { get => dockerPath; set => dockerPath = value; }

        public StringBuilder dockercomposeFileRepresentation;

        public string GenerateDockerComposeMySql(string containerName = "MySqlDatabase", string username = "MySqlUsernale", string password = "MySqlPassword", string dbName = "MySqlDatabaseName", bool configureTable = false)
        {
            string outputDirectory = Path.Combine(AppDomain.CurrentDomain.BaseDirectory, containerName);
            Directory.CreateDirectory(outputDirectory);

            StringBuilder dockerComposeContent = new StringBuilder();

            dockerComposeContent.AppendLine($"version: '3'");
            dockerComposeContent.AppendLine($"name: {containerName.ToLower()}_sql_database");
            dockerComposeContent.AppendLine();

            dockerComposeContent.AppendLine($"services:");
            dockerComposeContent.AppendLine($"  {containerName}:");
            dockerComposeContent.AppendLine($"    image: mysql:latest");
            dockerComposeContent.AppendLine($"    restart: always");
            dockerComposeContent.AppendLine($"    environment:");
            dockerComposeContent.AppendLine($"      MYSQL_ROOT_PASSWORD: {password}");
            dockerComposeContent.AppendLine($"      MYSQL_DATABASE :  {dbName}");
            dockerComposeContent.AppendLine($"      MYSQL_USER :  {username}");
            dockerComposeContent.AppendLine($"      MYSQL_PASSWORD : {password}");
            dockerComposeContent.AppendLine($"      MYSQL_LOG_CONSOLE: \"true\"");

            dockerComposeContent.AppendLine($"    ports:");
            dockerComposeContent.AppendLine($"      - '3306:3306'");
            dockerComposeContent.AppendLine($"    networks:");
            dockerComposeContent.AppendLine($"      - {containerName}_bridge_db");
            dockerComposeContent.AppendLine($"    volumes:");
            dockerComposeContent.AppendLine($"      - {containerName}_mysql-bd_data:/var/lib/mysql_db");

            if (configureTable)
            {
                dockerComposeContent.AppendLine($"      - ./init-script.sql:/docker-entrypoint-initdb.d/init-script.sql");
            }

            dockerComposeContent.AppendLine();

            dockerComposeContent.AppendLine($"networks:");
            dockerComposeContent.AppendLine($"  {containerName}_bridge_db:");
            dockerComposeContent.AppendLine($"    driver : bridge");
            dockerComposeContent.AppendLine($"    ipam:");
            dockerComposeContent.AppendLine($"      config:");
            dockerComposeContent.AppendLine($"        - subnet: \"172.20.0.0/24\"");

            dockerComposeContent.AppendLine();

            dockerComposeContent.AppendLine($"volumes:");
            dockerComposeContent.AppendLine($"  {containerName}_mysql-bd_data:");

            dockercomposeFileRepresentation = dockerComposeContent;

            // Write the content to a file
            string fullPath = Path.Combine(outputDirectory, "docker-compose.yml");
            File.WriteAllText(fullPath, dockerComposeContent.ToString());

            Console.WriteLine($"Docker compose generated and saved to {fullPath}");

            return outputDirectory; 
        }

        public string GenerateDockerComposeRedis(bool configurePersistence = false, string containerName = "RedisServer", string redisPassword = "test")
        {
            string outputDirectory = Path.Combine(AppDomain.CurrentDomain.BaseDirectory, containerName);
            Directory.CreateDirectory(outputDirectory);

            StringBuilder dockerComposeContent = new StringBuilder();

            dockerComposeContent.AppendLine($"version: '3'");
            dockerComposeContent.AppendLine($"name: {containerName.ToLower()}_redis_server");
            dockerComposeContent.AppendLine();

            dockerComposeContent.AppendLine($"services:");
            dockerComposeContent.AppendLine($"  {containerName}:");
            dockerComposeContent.AppendLine($"    image: redis:latest");
            dockerComposeContent.AppendLine($"    restart: always");

            if (configurePersistence)
            {
                dockerComposeContent.AppendLine($"   volumes:");
                dockerComposeContent.AppendLine($"      - {containerName}_redis-data:/data");
            }

            dockerComposeContent.AppendLine($"    environment:");
            dockerComposeContent.AppendLine($"      REDIS_PASSWORD: {redisPassword}");
            dockerComposeContent.AppendLine($"    ports:");
            dockerComposeContent.AppendLine($"      - '6379:6379'");
            dockerComposeContent.AppendLine($"    networks:");
            dockerComposeContent.AppendLine($"      - {containerName}_bridge_redis");
            dockerComposeContent.AppendLine();
            dockerComposeContent.AppendLine($"networks:");
            dockerComposeContent.AppendLine($"  {containerName}_bridge_redis:");
            dockerComposeContent.AppendLine($"    driver: bridge");
            dockerComposeContent.AppendLine($"    ipam:");
            dockerComposeContent.AppendLine($"      config:");
            dockerComposeContent.AppendLine($"        - subnet: '172.20.0.0/24'");
            dockerComposeContent.AppendLine();
            dockerComposeContent.AppendLine($"volumes:");

            if (configurePersistence)
            {
                dockerComposeContent.AppendLine($"  {containerName}_redis-data:");
            }

            // Write the content to a file
            string fullPath = Path.Combine(outputDirectory, "docker-compose.yml");
            File.WriteAllText(fullPath, dockerComposeContent.ToString());

            Console.WriteLine($"Docker compose generated and saved to {fullPath}");

            return outputDirectory; 
        }

        public string GenerateDockerComposeGitLab(string gitlabContainerName = "test", string gitLabPassword = "Test123456789*", string restart = "always", string gitlabNetworkName = "bridge_gitlab", string networkDriver = "bridge")
        {
            string outputDirectory = Path.Combine(AppDomain.CurrentDomain.BaseDirectory, gitlabContainerName);
            Directory.CreateDirectory(outputDirectory);

            StringBuilder dockerComposeContent = new StringBuilder();

            dockerComposeContent.AppendLine($"version: '3'");
            dockerComposeContent.AppendLine($"name: {gitlabContainerName.ToLower()}_gitlab_cicd");
            dockerComposeContent.AppendLine();

            dockerComposeContent.AppendLine($"services:");
            dockerComposeContent.AppendLine($"  {gitlabContainerName}:");
            dockerComposeContent.AppendLine($"    image: gitlab/gitlab-ce:latest");
            dockerComposeContent.AppendLine($"    restart: {restart}");
            dockerComposeContent.AppendLine($"    environment:");
            dockerComposeContent.AppendLine($"      GITLAB_OMNIBUS_CONFIG: |");
            dockerComposeContent.AppendLine($"        external_url 'http://{gitlabContainerName}.local'");
            dockerComposeContent.AppendLine($"        gitlab_rails['initial_root_password'] = '{gitLabPassword}'");
            dockerComposeContent.AppendLine($"    ports:");
            dockerComposeContent.AppendLine($"      - '80:80'");
            dockerComposeContent.AppendLine($"      - '443:443'");
            dockerComposeContent.AppendLine($"      - '22:22'");
            dockerComposeContent.AppendLine($"    networks:");
            dockerComposeContent.AppendLine($"      - {gitlabContainerName}_{gitlabNetworkName}");
            dockerComposeContent.AppendLine($"    volumes:");
            dockerComposeContent.AppendLine($"      - {gitlabContainerName}_gitlab_data:/var/opt/gitlab");
            dockerComposeContent.AppendLine($"      - {gitlabContainerName}_gitlab_logs:/var/log/gitlab");
            dockerComposeContent.AppendLine($"      - {gitlabContainerName}_gitlab_config:/etc/gitlab");
            dockerComposeContent.AppendLine();

            bool gitLabStageServer = UserInput.AskYesNoQuestion("Do you want to create a stage server (Ubuntu) :");

            string gitLabStageServerContainerName = "";
            string gitLabProdServerContainerName = "";

            if (gitLabStageServer)
            {
                gitLabStageServerContainerName = UserInput.GetUserInput("Enter stage server container name :");

                string gitLabStageServerRootPassword = UserInput.GetUserInput("Enter stage server root password :");

                dockerComposeContent.AppendLine($"  {gitLabStageServerContainerName}:");
                dockerComposeContent.AppendLine($"    image: ubuntu:latest");
                dockerComposeContent.AppendLine($"    restart: {restart}");
                dockerComposeContent.AppendLine($"    environment:");
                dockerComposeContent.AppendLine($"      - ROOT_PASSWORD={gitLabStageServerRootPassword}");
                dockerComposeContent.AppendLine($"    ports:");
                dockerComposeContent.AppendLine($"      - '8181:81'");
                dockerComposeContent.AppendLine($"    networks:");
                dockerComposeContent.AppendLine($"      - {gitlabContainerName}_{gitlabNetworkName}");
                dockerComposeContent.AppendLine($"    volumes:");
                dockerComposeContent.AppendLine($"      - {gitLabStageServerContainerName}_data:/app/data");
                dockerComposeContent.AppendLine($"    command: tail -f /dev/null");
                dockerComposeContent.AppendLine();
            }

            bool gitLabProdServer = UserInput.AskYesNoQuestion("Do you want to create a production server (Ubuntu) :");
            Console.WriteLine();

            if (gitLabProdServer)
            {
                gitLabProdServerContainerName = UserInput.GetUserInput("Enter production server container name :");

                string gitLabProdServerRootPassword = UserInput.GetUserInput("Enter production server root password :");

                dockerComposeContent.AppendLine($"  {gitLabProdServerContainerName}:");
                dockerComposeContent.AppendLine($"    image: ubuntu:latest");
                dockerComposeContent.AppendLine($"    restart: {restart}");
                dockerComposeContent.AppendLine($"    environment:");
                dockerComposeContent.AppendLine($"      - ROOT_PASSWORD={gitLabProdServerRootPassword}");
                dockerComposeContent.AppendLine($"    ports:");


                dockerComposeContent.AppendLine($"      - '8282:82'");
                dockerComposeContent.AppendLine($"    networks:");
                dockerComposeContent.AppendLine($"      - {gitlabContainerName}_{gitlabNetworkName}");
                dockerComposeContent.AppendLine($"    volumes:");
                dockerComposeContent.AppendLine($"      - {gitLabProdServerContainerName}_data:/app/data");
                dockerComposeContent.AppendLine($"    command: tail -f /dev/null");
                dockerComposeContent.AppendLine();
            }

            dockerComposeContent.AppendLine($"networks:");
            dockerComposeContent.AppendLine($"  {gitlabContainerName}_{gitlabNetworkName}:");
            dockerComposeContent.AppendLine($"    driver : {networkDriver}");
            dockerComposeContent.AppendLine($"    ipam:");
            dockerComposeContent.AppendLine($"      config:");
            dockerComposeContent.AppendLine($"        - subnet: \"172.21.0.0/24\"");

            dockerComposeContent.AppendLine($"volumes:");
            dockerComposeContent.AppendLine($"  {gitlabContainerName}_gitlab_data:");
            dockerComposeContent.AppendLine($"  {gitlabContainerName}_gitlab_logs:");
            dockerComposeContent.AppendLine($"  {gitlabContainerName}_gitlab_config:");

            if (gitLabStageServer)
                dockerComposeContent.AppendLine($"  {gitLabStageServerContainerName}_data:");

            if (gitLabProdServer)
                dockerComposeContent.AppendLine($"  {gitLabProdServerContainerName}_data:");

            // Write the content to a file
            string fullPath = Path.Combine(outputDirectory, "docker-compose.yml");
            File.WriteAllText(fullPath, dockerComposeContent.ToString());

            Console.WriteLine($"Docker compose generated and saved to {fullPath}");

            return outputDirectory; 
        }

        public string GenerateDockerComposeNextcloud(string dbUsername = "dbUsername", string containerName = "nextcloud", string dbContainerName = "NextcloudDB", string dbPassword = "db_password", string dbRootPassword = "root_password")
        {

            string outputDirectory = Path.Combine(AppDomain.CurrentDomain.BaseDirectory, containerName);
            Directory.CreateDirectory(outputDirectory);

            //StringBuilder nginxConfContent = new StringBuilder();

            //// Ajouter le code pour générer le fichier nginx.conf
            //nginxConfContent.AppendLine($"user  nginx;");
            //nginxConfContent.AppendLine($"worker_processes  auto;");
            //nginxConfContent.AppendLine($"error_log  /var/log/nginx/error.log notice;");
            //nginxConfContent.AppendLine($"pid        /var/run/nginx.pid;");
            //nginxConfContent.AppendLine();
            //nginxConfContent.AppendLine("events {");
            //nginxConfContent.AppendLine($"    worker_connections  1024;");
            //nginxConfContent.AppendLine("}");
            //nginxConfContent.AppendLine();
            //nginxConfContent.AppendLine("http {");
            //nginxConfContent.AppendLine($"    include       /etc/nginx/mime.types;");
            //nginxConfContent.AppendLine($"    default_type  application/octet-stream;");
            //nginxConfContent.AppendLine();
            //nginxConfContent.AppendLine($"    log_format  main  '$remote_addr - $remote_user [$time_local] \"$request\" '");
            //nginxConfContent.AppendLine($"                      '$status $body_bytes_sent \"$http_referer\" '");
            //nginxConfContent.AppendLine($"                      '\"$http_user_agent\" \"$http_x_forwarded_for\"';");
            //nginxConfContent.AppendLine();
            //nginxConfContent.AppendLine($"    access_log  /var/log/nginx/access.log  main;");
            //nginxConfContent.AppendLine();
            //nginxConfContent.AppendLine($"    sendfile        on;");
            //nginxConfContent.AppendLine($"    keepalive_timeout  65;");
            //nginxConfContent.AppendLine();
            //nginxConfContent.AppendLine($"    include /etc/nginx/conf.d/*.conf;");
            //nginxConfContent.AppendLine();
            //nginxConfContent.AppendLine($"    upstream docker-nextcloud {{ server {containerName.ToLower()}-nextcloud-1:8080; }}");
            //nginxConfContent.AppendLine();
            //nginxConfContent.AppendLine("     # Proxy configuration for Next Cloud");
            //nginxConfContent.AppendLine($"    server {{");
            //nginxConfContent.AppendLine($"        listen 8080; # Utilisez le port que vous préférez");
            //nginxConfContent.AppendLine($"        server_name nextcloudserver;");
            //nginxConfContent.AppendLine();
            //nginxConfContent.AppendLine($"        location / {{");
            //nginxConfContent.AppendLine($"            proxy_pass http://docker-nextcloud;");
            //nginxConfContent.AppendLine($"            proxy_redirect off;");
            //nginxConfContent.AppendLine($"            proxy_set_header Host $host;");
            //nginxConfContent.AppendLine($"            proxy_set_header X-Real-IP $remote_addr;");
            //nginxConfContent.AppendLine($"            proxy_set_header X-Forwarded-For $proxy_add_x_forwarded_for;");
            //nginxConfContent.AppendLine($"            proxy_set_header X-Forwarded-Host $server_name;");
            //nginxConfContent.AppendLine($"        }}");
            //nginxConfContent.AppendLine();
            //nginxConfContent.AppendLine($"        # Ajoutez d'autres configurations si nécessaire");
            //nginxConfContent.AppendLine($"    }}");
            //nginxConfContent.AppendLine($"}}");

            //string nginxConfPath = Path.Combine(outputDirectory, "nginx.conf");
            //File.WriteAllText(nginxConfPath, nginxConfContent.ToString());

            StringBuilder dockerComposeContent = new StringBuilder();

            dockerComposeContent.AppendLine($"version: '3'");
            dockerComposeContent.AppendLine($"name: {containerName.ToLower()}");
            dockerComposeContent.AppendLine();

            dockerComposeContent.AppendLine($"services:");

            //dockerComposeContent.AppendLine($"  nginx:");
            //dockerComposeContent.AppendLine($"    image: nginx:latest");
            //dockerComposeContent.AppendLine($"    restart: always");
            //dockerComposeContent.AppendLine($"    ports:");
            //dockerComposeContent.AppendLine($"      - '8080:80'");
            //dockerComposeContent.AppendLine($"    volumes:");
            //dockerComposeContent.AppendLine($"      - ./nginx.conf:/etc/nginx/nginx.conf");

            dockerComposeContent.AppendLine($"  nextcloudDb:");
            dockerComposeContent.AppendLine($"    image: mysql:latest");
            dockerComposeContent.AppendLine($"    restart: always");
            dockerComposeContent.AppendLine($"    environment:");
            dockerComposeContent.AppendLine($"      MYSQL_ROOT_PASSWORD: {dbRootPassword}");
            dockerComposeContent.AppendLine($"      MYSQL_DATABASE: nextcloud");
            dockerComposeContent.AppendLine($"      MYSQL_USER: {dbUsername}");
            dockerComposeContent.AppendLine($"      MYSQL_PASSWORD: {dbPassword}");
            dockerComposeContent.AppendLine($"      MYSQL_LOG_CONSOLE: \"true\"");
            dockerComposeContent.AppendLine($"    networks:");
            dockerComposeContent.AppendLine($"      - {containerName.ToLower()}_network");
            dockerComposeContent.AppendLine($"    volumes:");
            dockerComposeContent.AppendLine($"      - {dbContainerName.ToLower()}_data:/var/lib/mysql");

            dockerComposeContent.AppendLine($"  nextcloud:");
            dockerComposeContent.AppendLine($"    image: nextcloud:latest");
            dockerComposeContent.AppendLine($"    restart: always");
            dockerComposeContent.AppendLine($"    environment:");
            dockerComposeContent.AppendLine($"      MYSQL_DATABASE: nextcloud");
            dockerComposeContent.AppendLine($"      MYSQL_USER: {dbUsername}");
            dockerComposeContent.AppendLine($"      MYSQL_PASSWORD: {dbPassword}");
            dockerComposeContent.AppendLine($"      MYSQL_HOST: {dbContainerName}");
            dockerComposeContent.AppendLine($"    ports:");
            dockerComposeContent.AppendLine($"      - '8080:80'");
            dockerComposeContent.AppendLine($"    networks:");
            dockerComposeContent.AppendLine($"      - {containerName.ToLower()}_network");
            dockerComposeContent.AppendLine($"    volumes:");
            dockerComposeContent.AppendLine($"      - {containerName.ToLower()}_data:/var/www/html");

            dockerComposeContent.AppendLine($"volumes:");
            dockerComposeContent.AppendLine($"  {dbContainerName.ToLower()}_data:");
            dockerComposeContent.AppendLine($"  {containerName.ToLower()}_data:");

            // Définition du réseau
            dockerComposeContent.AppendLine($"networks:");
            dockerComposeContent.AppendLine($"  {containerName.ToLower()}_network:");
            dockerComposeContent.AppendLine($"    driver: bridge");
            dockerComposeContent.AppendLine($"    ipam:");
            dockerComposeContent.AppendLine($"      config:");
            dockerComposeContent.AppendLine($"        - subnet: \"172.21.0.0/24\"");

            // Write the content to a file
            string fullPath = Path.Combine(outputDirectory, "docker-compose.yml");
            File.WriteAllText(fullPath, dockerComposeContent.ToString());

            Console.WriteLine($"Docker compose for Nextcloud with MySQL and Nginx proxy generated and saved to {fullPath}");

            return outputDirectory;
        }

        public string GenerateDockerComposeJupyterNotebook(string containerName = "JupyterNotebook", int port = 8888, string password = "your_password")
        {
            string outputDirectory = Path.Combine(AppDomain.CurrentDomain.BaseDirectory, containerName);
            Directory.CreateDirectory(outputDirectory);

            StringBuilder dockerComposeContent = new StringBuilder();

            dockerComposeContent.AppendLine($"version: '3'");
            dockerComposeContent.AppendLine($"services:");
            dockerComposeContent.AppendLine($"  {containerName}:");
            dockerComposeContent.AppendLine($"    image: jupyter/base-notebook:latest");
            dockerComposeContent.AppendLine($"    restart: always");
            dockerComposeContent.AppendLine($"    ports:");
            dockerComposeContent.AppendLine($"      - '{port}:8888'");
            dockerComposeContent.AppendLine($"    environment:");
            dockerComposeContent.AppendLine($"      JUPYTER_ENABLE_LAB: 'yes'");
            dockerComposeContent.AppendLine($"      JUPYTER_TOKEN: {password}");

            // Write the content to a file
            string fullPath = Path.Combine(outputDirectory, "docker-compose.yml");
            File.WriteAllText(fullPath, dockerComposeContent.ToString());

            Console.WriteLine($"Docker compose for Jupyter Notebook generated and saved to {fullPath}");

            return outputDirectory; 
        }

        public string GenerateDockerComposeJupyerInNginx(string jupyterContainerName = "jupyter", int jupyterPort = 8888, string networkName = "JupyterNetwork", int nginxPort = 8080)
        {
            string outputDirectory = Path.Combine(AppDomain.CurrentDomain.BaseDirectory, jupyterContainerName);
            Directory.CreateDirectory(outputDirectory);

            // Construction du contenu du fichier nginx.conf
            StringBuilder nginxConfContent = new StringBuilder();

            nginxConfContent.AppendLine($"user  nginx;");
            nginxConfContent.AppendLine($"worker_processes  auto;");
            nginxConfContent.AppendLine($"error_log  /var/log/nginx/error.log notice;");
            nginxConfContent.AppendLine($"pid        /var/run/nginx.pid;");
            nginxConfContent.AppendLine();
            nginxConfContent.AppendLine("events {");
            nginxConfContent.AppendLine($"    worker_connections  1024;");
            nginxConfContent.AppendLine("}");
            nginxConfContent.AppendLine();
            nginxConfContent.AppendLine("http {");
            nginxConfContent.AppendLine($"    include       /etc/nginx/mime.types;");
            nginxConfContent.AppendLine($"    default_type  application/octet-stream;");
            nginxConfContent.AppendLine();
            nginxConfContent.AppendLine($"    log_format  main  '$remote_addr - $remote_user [$time_local] \"$request\" '");
            nginxConfContent.AppendLine($"                      '$status $body_bytes_sent \"$http_referer\" '");
            nginxConfContent.AppendLine($"                      '\"$http_user_agent\" \"$http_x_forwarded_for\"';");
            nginxConfContent.AppendLine();
            nginxConfContent.AppendLine($"    access_log  /var/log/nginx/access.log  main;");
            nginxConfContent.AppendLine();
            nginxConfContent.AppendLine($"    sendfile        on;");
            nginxConfContent.AppendLine($"    keepalive_timeout  65;");
            nginxConfContent.AppendLine();
            nginxConfContent.AppendLine($"    include /etc/nginx/conf.d/*.conf;");
            nginxConfContent.AppendLine();
            nginxConfContent.AppendLine($"    upstream docker-jupyter {{ server {jupyterContainerName}-jupyter-1:{jupyterPort}; }}");
            nginxConfContent.AppendLine();
            nginxConfContent.AppendLine("     # Proxy configuration for Jupyter");
            nginxConfContent.AppendLine($"    server {{");
            nginxConfContent.AppendLine($"        listen 8888; # Utilisez le port que vous préférez");
            nginxConfContent.AppendLine($"        server_name jupyterserver;");
            nginxConfContent.AppendLine();
            nginxConfContent.AppendLine($"        location / {{");
            nginxConfContent.AppendLine($"            proxy_pass http://docker-jupyter;");
            nginxConfContent.AppendLine($"            proxy_redirect off;");
            nginxConfContent.AppendLine($"            proxy_set_header Host $host;");
            nginxConfContent.AppendLine($"            proxy_set_header X-Real-IP $remote_addr;");
            nginxConfContent.AppendLine($"            proxy_set_header X-Forwarded-For $proxy_add_x_forwarded_for;");
            nginxConfContent.AppendLine($"            proxy_set_header X-Forwarded-Host $server_name;");
            nginxConfContent.AppendLine($"        }}");
            nginxConfContent.AppendLine();
            nginxConfContent.AppendLine($"        # Ajoutez d'autres configurations si nécessaire");
            nginxConfContent.AppendLine($"    }}");
            nginxConfContent.AppendLine($"}}");

            string nginxConfPath = Path.Combine(outputDirectory, "nginx.conf");
            File.WriteAllText(nginxConfPath, nginxConfContent.ToString());

            // Construction du contenu du fichier index.html
            string indexHtmlContent = @"
                            <!DOCTYPE html>
                            <html lang=""en"">

                            <head>
                                <meta charset=""UTF-8"">
                                <meta name=""viewport"" content=""width=device-width, initial-scale=1.0"">
                                <title>Bienvenue sur Votre Serveur de Développement</title>
                                <style>
                                    body {
                                        font-family: Arial, sans-serif;
                                        background-color: #f4f4f4;
                                        margin: 0;
                                        padding: 0;
                                        display: flex;
                                        flex-direction: column;
                                        align-items: center;
                                        justify-content: center;
                                        height: 100vh;
                                    }

                                    .container {
                                        text-align: center;
                                        max-width: 600px;
                                    }

                                    h1 {
                                        color: #333;
                                    }

                                    p {
                                        color: #555;
                                        margin-top: 10px;
                                    }

                                    .button {
                                        display: inline-block;
                                        background-color: #4caf50;
                                        color: white;
                                        padding: 10px 20px;
                                        text-decoration: none;
                                        font-size: 16px;
                                        border-radius: 5px;
                                        margin-top: 20px;
                                    }
                                </style>
                            </head>

                            <body>
                                <div class=""container"">
                                    <h1>Bienvenue sur Votre Serveur de Développement</h1>
                                    <p>Votre serveur est actuellement en développement. Profitez de l'environnement de développement sécurisé Jupyter afin de patienter.</p>
                                    <a class=""button"" href=""http://localhost:8888"" target=""_blank"">Accéder à Jupyter</a>
                                </div>
                            </body>

                            </html>";

            string indexPath = Path.Combine(outputDirectory, "index.html");
            File.WriteAllText(indexPath, indexHtmlContent);

            StringBuilder dockerComposeContent = new StringBuilder();

            dockerComposeContent.AppendLine("version: '3'");
            dockerComposeContent.AppendLine($"name: {jupyterContainerName.ToLower()}");
            dockerComposeContent.AppendLine("services:");

            // Nginx service
            dockerComposeContent.AppendLine($"  nginx:");
            dockerComposeContent.AppendLine($"    image: nginx:latest");
            dockerComposeContent.AppendLine($"    ports:");
            dockerComposeContent.AppendLine($"      - '8080:8080'");
            dockerComposeContent.AppendLine($"      - '8888:8888'");
            dockerComposeContent.AppendLine($"    networks:");
            dockerComposeContent.AppendLine($"      - {networkName}");
            dockerComposeContent.AppendLine($"    volumes:");
            dockerComposeContent.AppendLine($"      - ./nginx.conf:/etc/nginx/nginx.conf");
            dockerComposeContent.AppendLine($"      - ./index.html:/usr/share/nginx/html/index.html"); // Mounting index.html


            // Service Jupyter
            dockerComposeContent.AppendLine($"  jupyter:");
            dockerComposeContent.AppendLine($"    image: jupyter/base-notebook:latest");
            dockerComposeContent.AppendLine($"    restart: always");
            dockerComposeContent.AppendLine($"    environment:");
            dockerComposeContent.AppendLine($"      JUPYTER_ENABLE_LAB: 'yes'");
            dockerComposeContent.AppendLine($"      JUPYTER_TOKEN: Test1234456789*"); // You need to implement GenerateRandomToken()
            dockerComposeContent.AppendLine($"    networks:");
            dockerComposeContent.AppendLine($"      - {networkName}");
            dockerComposeContent.AppendLine($"    volumes:");
            dockerComposeContent.AppendLine($"      - jupyter_data:/home/jovyan"); // Mounting a volume for persistence


            // Définition du volume
            dockerComposeContent.AppendLine($"volumes:");
            dockerComposeContent.AppendLine($"  jupyter_data:");

            // Définition du réseau
            dockerComposeContent.AppendLine($"networks:");
            dockerComposeContent.AppendLine($"  {networkName}:");
            dockerComposeContent.AppendLine($"    driver: bridge");
            dockerComposeContent.AppendLine($"    ipam:");
            dockerComposeContent.AppendLine($"      config:");
            dockerComposeContent.AppendLine($"        - subnet: \"172.21.0.0/24\"");

            // Écriture du contenu dans un fichier
            string fullPath = Path.Combine(outputDirectory, "docker-compose.yml");
            File.WriteAllText(fullPath, dockerComposeContent.ToString());

            Console.WriteLine($"Docker compose with Nginx for Jupyter, volumes, and network generated and saved to {fullPath}");
            Console.WriteLine($"Nginx configuration file generated and saved to {nginxConfPath}");

            return outputDirectory; 
        }
    }
}
