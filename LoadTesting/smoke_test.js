import http from 'k6/http';
import { check, sleep } from 'k6';

export const options = {
    insecureSkipTLSVerify: true,
    noConnectionReuse: false,
    vus: 3, // Key for Smoke test. Keep it at 2, 3, max 5 VUs
    duration: '1m', // This can be shorter or just a few iterations
};

export default () => {
    const urlRes = http.get('https://localhost:7057/Hasher/ValidateHash');
    // Check response status and content
    const statusCheck = check(response, {
        'Status is 200': (r) => r.status === 200,
        'Last character is a number and odd': (r) => {
            const lastChar = r.body.slice(-1);
            return !isNaN(lastChar) && parseInt(lastChar) % 2 !== 0;
        },
    });

    // Log results of status checks
    if (!statusCheck) {
        console.error('Some checks failed:', response.status);
    }
    sleep(1);
};
