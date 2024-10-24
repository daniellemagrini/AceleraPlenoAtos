FROM node:20.14.0

# Defina o diretório de trabalho dentro do contêiner
WORKDIR /app

# Copie o arquivo package.json e package-lock.json
COPY package*.json ./

# Instale as dependências
RUN npm install
RUN npm install -g @angular/cli@17.1.2

# Copie o código-fonte do projeto
COPY . .

# Exponha a porta em que o aplicativo estará ouvindo
EXPOSE 80

# Execute o servidor Angular
CMD ["ng", "serve", "--host", "0.0.0.0", "--port", "80"]