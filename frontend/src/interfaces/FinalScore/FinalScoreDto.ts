export interface FinalScoreDto {
  id: string;
  score: number;
  companyScoreX2: number;
  aiScoreX1: number;
  x1X2Average: number;
  correlation: number;
  correlationBoostValue: number;
  correlationBoostModifer: number;
  review: string;
}
