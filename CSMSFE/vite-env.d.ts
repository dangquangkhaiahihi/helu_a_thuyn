interface ImportMetaEnv {
  readonly VITE_ENV: string;
  readonly VITE_API_ENDPOINT: string;
  readonly VITE_BASE_DOMAIN: string;
  readonly VITE_MEDIA_URL: string;
  readonly VITE_SPECKLE_DOMAIN: string;
  readonly VITE_SPECKLE_TOKEN_READ: string;
}

interface ImportMeta {
  readonly env: ImportMetaEnv;
}
