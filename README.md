# ShopSphere 🛒

> Application e-commerce moderne fullstack avec React, TypeScript, .NET 8 et PostgreSQL

[![Live Demo](https://img.shields.io/badge/demo-coming_soon-yellow.svg)]()
[![Build Status](https://img.shields.io/badge/build-passing-brightgreen.svg)]()
[![License](https://img.shields.io/badge/license-MIT-blue.svg)]()

![ShopSphere](https://via.placeholder.com/800x400/3B82F6/FFFFFF?text=ShopSphere+E-commerce)

## 📋 Table des matières

- [À propos](#à-propos)
- [Fonctionnalités](#fonctionnalités)
- [Technologies utilisées](#technologies-utilisées)
- [Démarrage rapide](#démarrage-rapide)
- [Installation](#installation)
- [Structure du projet](#structure-du-projet)
- [API Documentation](#api-documentation)
- [Tests](#tests)
- [Déploiement](#déploiement)

---

## 🎯 À propos

**ShopSphere** est une application e-commerce complète construite avec les technologies modernes. Ce projet démontre une architecture fullstack professionnelle avec React frontend et .NET backend.

**Contexte du projet** : Projet portfolio pour démontrer mes compétences en développement fullstack

**Objectifs d'apprentissage** :
- Maîtriser React avec TypeScript et Redux Toolkit
- Développer une API RESTful avec .NET 8
- Implémenter un système d'authentification JWT
- Intégrer un système de paiement (Stripe)
- Containeriser avec Docker

---

## ✨ Fonctionnalités

### Fonctionnalités principales

- ✅ **Authentification complète** : Inscription, connexion, JWT tokens
- ✅ **Catalogue produits** : Liste, recherche, filtres, pagination
- ✅ **Panier d'achat** : Gestion temps réel du panier
- ✅ **Système de commandes** : Checkout et historique
- ✅ **Paiement Stripe** : Intégration sécurisée
- ✅ **Interface responsive** : Design mobile-first

### Fonctionnalités techniques

- 🔐 **Authentification JWT** avec refresh tokens
- 🎨 **Interface responsive** avec TailwindCSS
- ⚡ **Performance optimisée** avec lazy loading
- 🧪 **Tests** unitaires et d'intégration
- 🐳 **Dockerisé** pour un déploiement facile
- 📊 **Dashboard Admin** (bonus)

---

## 🛠️ Technologies utilisées

### Frontend
- **Framework** : React 18
- **Language** : TypeScript
- **Styling** : TailwindCSS
- **State Management** : Redux Toolkit
- **HTTP Client** : Axios
- **Routing** : React Router v6
- **Forms** : React Hook Form + Zod
- **Testing** : Jest, React Testing Library

### Backend
- **Framework** : .NET 8 Web API
- **Database** : PostgreSQL
- **ORM** : Entity Framework Core
- **Authentication** : JWT
- **Payment** : Stripe
- **API Documentation** : Swagger/OpenAPI
- **Testing** : xUnit, Moq

### DevOps
- **Containerization** : Docker, Docker Compose
- **CI/CD** : GitHub Actions
- **Hosting** : Heroku / Azure (à venir)

---

## 🚀 Démarrage rapide

### Prérequis

- Node.js v20+
- .NET 8 SDK
- PostgreSQL 15+
- Docker (optionnel)
- Compte Stripe (pour les paiements)

### Installation rapide avec Docker

```bash
# Cloner le repo
git clone https://github.com/votre-username/shopsphere.git
cd shopsphere

# Configuration
cp .env.example .env
# Éditer .env avec vos valeurs

# Lancer avec Docker Compose
docker-compose up -d

# L'application est accessible sur:
# Frontend: http://localhost:3000
# Backend API: http://localhost:5000
# Swagger: http://localhost:5000/swagger
```

---

## 📦 Installation manuelle

### 1. Cloner le repository

```bash
git clone https://github.com/votre-username/shopsphere.git
cd shopsphere
```

### 2. Configuration de l'environnement

Créer un fichier `.env` à la racine :

```env
# Database
DATABASE_URL="Host=localhost;Database=shopsphere;Username=postgres;Password=yourpassword"

# JWT
JWT_SECRET=your-super-secret-jwt-key-change-this-in-production
JWT_EXPIRATION_MINUTES=15
JWT_REFRESH_EXPIRATION_DAYS=7

# Stripe
STRIPE_SECRET_KEY=sk_test_your_stripe_secret_key
STRIPE_PUBLISHABLE_KEY=pk_test_your_stripe_publishable_key

# Frontend
REACT_APP_API_URL=http://localhost:5000
REACT_APP_STRIPE_PUBLISHABLE_KEY=pk_test_your_stripe_publishable_key
```

### 3. Installation Backend (.NET)

```bash
cd backend

# Restaurer les packages
dotnet restore

# Créer la base de données
dotnet ef database update --project ShopSphere.Infrastructure

# Lancer l'API
dotnet run --project ShopSphere.API

# API disponible sur http://localhost:5000
# Swagger sur http://localhost:5000/swagger
```

### 4. Installation Frontend (React)

```bash
cd frontend

# Installer les dépendances
npm install

# Lancer en mode développement
npm run dev

# Application disponible sur http://localhost:3000
```

### 5. Seed des données (optionnel)

```bash
cd backend
dotnet run --project ShopSphere.API -- seed

# Crée des catégories et produits de test
```

---

## 💻 Utilisation

### Comptes de test

```
Admin:
email: admin@shopsphere.com
password: Admin123!

Customer:
email: demo@shopsphere.com
password: Demo123!
```

### Carte de test Stripe

```
Numéro: 4242 4242 4242 4242
Date: N'importe quelle date future
CVC: N'importe quel 3 chiffres
```

---

## 📁 Structure du projet

```
ShopSphere/
├── backend/                        # .NET 8 API
│   ├── ShopSphere.API/            # Controllers & Middleware
│   ├── ShopSphere.Core/           # Business Logic & Services
│   ├── ShopSphere.Infrastructure/ # Data Access & Repositories
│   └── ShopSphere.Tests/          # Tests
│
├── frontend/                       # React + TypeScript
│   ├── src/
│   │   ├── components/            # Composants réutilisables
│   │   ├── pages/                 # Pages de l'application
│   │   ├── store/                 # Redux store
│   │   ├── services/              # API calls
│   │   └── types/                 # TypeScript types
│   └── public/
│
├── docker-compose.yml
├── .env.example
└── README.md
```

---

## 📚 API Documentation

### Base URL
```
http://localhost:5000/api
```

### Endpoints principaux

#### Authentication
```http
POST   /api/auth/register       # Inscription
POST   /api/auth/login          # Connexion
POST   /api/auth/refresh        # Refresh token
```

#### Products
```http
GET    /api/products            # Liste des produits
GET    /api/products/{id}       # Détail d'un produit
GET    /api/products/search     # Recherche
```

#### Cart
```http
GET    /api/cart                # Obtenir le panier
POST   /api/cart/items          # Ajouter au panier
PUT    /api/cart/items/{id}     # Modifier quantité
DELETE /api/cart/items/{id}     # Retirer du panier
```

#### Orders
```http
GET    /api/orders              # Historique
GET    /api/orders/{id}         # Détail
POST   /api/orders              # Créer commande
POST   /api/orders/{id}/pay     # Payer (Stripe)
```

**Documentation complète** : http://localhost:5000/swagger

---

## 🧪 Tests

### Backend

```bash
cd backend

# Tous les tests
dotnet test

# Tests unitaires uniquement
dotnet test --filter Category=Unit

# Tests d'intégration
dotnet test --filter Category=Integration

# Coverage
dotnet test /p:CollectCoverage=true /p:CoverletOutputFormat=opencover
```

### Frontend

```bash
cd frontend

# Tous les tests
npm test

# Mode watch
npm test -- --watch

# Coverage
npm test -- --coverage
```

---

## 🚢 Déploiement

### Docker

```bash
# Build les images
docker-compose build

# Déployer
docker-compose up -d

# Logs
docker-compose logs -f
```

### Heroku (Backend)

```bash
# Login
heroku login

# Créer l'app
heroku create shopsphere-api

# Ajouter PostgreSQL
heroku addons:create heroku-postgresql:hobby-dev

# Configurer les variables
heroku config:set JWT_SECRET=your-secret
heroku config:set STRIPE_SECRET_KEY=your-key

# Déployer
git push heroku main

# Migrations
heroku run dotnet ef database update
```

### Vercel (Frontend)

```bash
# Install Vercel CLI
npm install -g vercel

# Déployer
cd frontend
vercel --prod
```

---

## 🗺️ Roadmap

### Version 1.0 (Actuelle)
- [x] Authentification JWT
- [x] CRUD produits
- [x] Panier fonctionnel
- [x] Système de commandes
- [x] Paiement Stripe
- [x] Interface responsive

### Version 1.1 (À venir)
- [ ] Dashboard Admin
- [ ] Gestion des stocks
- [ ] Notifications email
- [ ] Système de reviews
- [ ] Wishlist
- [ ] Multi-devise

### Version 2.0 (Futur)
- [ ] Recommandations IA
- [ ] Application mobile React Native
- [ ] Multi-vendeurs
- [ ] Analytics avancés

---

## 👤 Auteur

**Nils Blandel**

- GitHub: [@ZuLoiZo](https://github.com/ZuLoiZo)
- LinkedIn: [Blandel Nils](https://linkedin.com/in/blandel.nils)
- Email: blandel.nils@gmail.com

## 📝 Licence

Ce projet est sous licence MIT. Voir le fichier [LICENSE](LICENSE) pour plus de détails.
---

## 🙏 Remerciements

- [Le Wagon](https://www.lewagon.com/) pour la formation
- [Stripe](https://stripe.com/) pour la documentation
- La communauté open source

---

⭐️ N'oublie pas de star ce repo si tu l'as trouvé utile !

Made with ❤️ by Nils Blandel
